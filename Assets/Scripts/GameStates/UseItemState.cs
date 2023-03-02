using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.StateMachine;

public class UseItemState : State<GameController>
{
    [SerializeField] PartyScreen partyScreen;
    [SerializeField] InventoryUI inventoryUI;

    public static UseItemState i { get; private set; }
    Inventory inventory;

    private void Awake()
    {
        i = this;
        inventory = Inventory.GetInventory();
    }

    GameController gc;

    public override void Enter(GameController owner)
    {
        gc = owner;

        StartCoroutine(UseItem());
    }

    IEnumerator UseItem()
    {      
        var item = inventoryUI.SelectedItem;
        var pokemon = partyScreen.SelectedMember;

        if (item is TmItem)
        {
            yield return HandleTmItems();
        }
        else
        {
            // Handle Evolution Items
            if (item is EvolutionItem)
            {
                var evolution = pokemon.CheckForEvolution(item);
                if (evolution != null)
                {
                    yield return EvolutionManager.i.Evolve(pokemon, evolution);
                }
                else
                {
                    yield return DialogueManager.Instance.ShowDialogText($"It has no effect.");
                    gc.StateMachine.Pop();
                    yield break;
                }
            }

            var usedItem = inventory.UseItem(item, partyScreen.SelectedMember);
            if (usedItem != null)
            {
                if (usedItem is RecoveryItem)
                {
                    yield return DialogueManager.Instance.ShowDialogText($"The player used { usedItem.Name }.");
                }
            }
            else
            {
                if (inventoryUI.SelectedCategory == (int)ItemCategory.Items)
                {
                    yield return DialogueManager.Instance.ShowDialogText($"It has no effect.");
                }
            }
        }

        gc.StateMachine.Pop();
    }

    IEnumerator HandleTmItems()
    {
        var tmItem = inventoryUI.SelectedItem as TmItem;
        if (tmItem == null)
        {
            yield break;
        }

        var pokemon = partyScreen.SelectedMember;

        if (pokemon.HasMove(tmItem.Move))
        {
            yield return DialogueManager.Instance.ShowDialogText($"{ pokemon.Base.Name } already knows { tmItem.Move.Name }!");
            yield break;
        }

        if (!tmItem.CanBeTaught(pokemon))
        {
            yield return DialogueManager.Instance.ShowDialogText($"{ pokemon.Base.Name } can't learn { tmItem.Move.Name }!");
            yield break;
        }

        if (pokemon.Moves.Count < PokemonBase.MaxNumOfMoves)
        {
            pokemon.LearnMove(tmItem.Move);
            yield return DialogueManager.Instance.ShowDialogText($"{ pokemon.Base.Name } learned { tmItem.Move.Name }!");
        }
        else
        {
            yield return DialogueManager.Instance.ShowDialogText($"{pokemon.Base.Name} is trying to learn {tmItem.Move.Name}.");
            yield return DialogueManager.Instance.ShowDialogText($"But {pokemon.Base.Name} cannot learn more than {PokemonBase.MaxNumOfMoves} moves.");
            //yield return ChooseMoveToForget(pokemon, tmItem.Move);
            //yield return new WaitUntil(() => state != InventoryUIState.MoveToForget);
        }
    }
}

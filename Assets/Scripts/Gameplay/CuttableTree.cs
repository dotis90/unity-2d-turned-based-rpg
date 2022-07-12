using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CuttableTree : MonoBehaviour, IInteractable
{
    public IEnumerator Interact(Transform initiator)
    {
        yield return DialogueManager.Instance.ShowDialogText("This tree looks like it can be cut");

        var pokemonWithCut = initiator.GetComponent<PokemonParty>().Pokemons.FirstOrDefault(p => p.Moves.Any(m => m.Base.Name == "Cut"));

        if (pokemonWithCut != null)
        {
            int selectedChoice = 0;
            yield return DialogueManager.Instance.ShowDialogText($"Should { pokemonWithCut.Base.Name } use cut?", 
                choices: new List<string>() { "Yes", "No"}, 
                onChoiceSelected: (selection) => selectedChoice = selection);

            if (selectedChoice == 0)
            {
                // Yes               
                yield return DialogueManager.Instance.ShowDialogText($"{ pokemonWithCut.Base.Name } used Cut!");
                gameObject.SetActive(false);
            }
        }
    }
}

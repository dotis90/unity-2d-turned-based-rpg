using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonGiver : MonoBehaviour
{
    [SerializeField] Pokemon pokemonToGive;
    [SerializeField] Dialogue dialog;

    bool used = false;

    public IEnumerator GivePokemon(PlayerController player)
    {
        yield return DialogueManager.Instance.ShowDialog(dialog);

        pokemonToGive.Init();
        player.GetComponent<PokemonParty>().AddPokemon(pokemonToGive);

        used = true;

        string dialogText = $"{ player.Name } received { pokemonToGive.Base.Name }.";

        yield return DialogueManager.Instance.ShowDialogText(dialogText);
    }

    public bool CanBeGiven()
    {
        return pokemonToGive != null && !used;
    }

    public object CaptureState()
    {
        return used;
    }

    public void RestoreState(object state)
    {
        used = (bool)state;
    }
}

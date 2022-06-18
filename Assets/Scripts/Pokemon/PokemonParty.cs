using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    [SerializeField] List<Pokemon> pokemon;

    public List<Pokemon> Pokemons
    {
        get
        {
            return pokemon;
        }
        set
        {
            pokemon = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (var p in pokemon)
        {
            p.Init();
        }
    }

    public Pokemon GetHealthyPokemon()
    {
        return pokemon.Where(x => x.HP > 0).FirstOrDefault();
    }

    public void AddPokemon(Pokemon newPokemon)
    {
        if (pokemon.Count < 6)
        {
            pokemon.Add(newPokemon);
        }
        else
        {
            // TODO: Add to the PC once that's implemented
        }
    }
}

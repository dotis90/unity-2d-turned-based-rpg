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

    // Update is called once per frame
    void Update()
    {
        
    }
}

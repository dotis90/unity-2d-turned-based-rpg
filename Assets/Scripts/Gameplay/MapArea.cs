using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField] List<Pokemon> wildPokemon;

    public Pokemon GetWildPokemon()
    {
        var pokemon = wildPokemon[Random.Range(0, wildPokemon.Count)];
        pokemon.Init();
        return pokemon;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

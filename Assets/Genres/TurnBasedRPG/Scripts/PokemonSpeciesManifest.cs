using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedRPG;

public class PokemonSpeciesManifest
{
    private List<PokemonSpecies> _manifest = new List<PokemonSpecies>();

    public PokemonSpeciesManifest()
    {
        _manifest = Resources.LoadAll("Pokemons", typeof(PokemonSpecies)).Cast<PokemonSpecies>().ToList();
    }

    public PokemonSpecies GetCreatureSpecies(PokemonSpeciesName name)
    {
        return _manifest.FirstOrDefault(species => species.Name == name);
    }

    public PokemonSpecies GetRandomCreatureSpecies()
    {
        return _manifest[Random.Range(0, _manifest.Count)];
    }
}
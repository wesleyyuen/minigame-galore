using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedRPG;

public class PokemonSpeciesManifest
{
    private HashSet<PokemonSpecies> _manifest = new HashSet<PokemonSpecies>();

    public PokemonSpeciesManifest(IPokemonSpeciesLoader loader)
    {
        _manifest = loader.Load("Pokemons").ToHashSet();
    }

    public PokemonSpecies GetCreatureSpecies(PokemonSpeciesName name)
    {
        return _manifest.FirstOrDefault(species => species.Name == name);
    }

    public PokemonSpecies GetRandomCreatureSpecies()
    {
        return _manifest.ElementAtOrDefault(Random.Range(0, _manifest.Count));
    }
}
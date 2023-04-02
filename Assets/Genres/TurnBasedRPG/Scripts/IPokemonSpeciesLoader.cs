using System.Collections.Generic;

namespace TurnBasedRPG
{
    public interface IPokemonSpeciesLoader
    {
        IEnumerable<PokemonSpecies> Load(string path);
        IAsyncEnumerable<PokemonSpecies> AsyncLoad(string path);
    }
}
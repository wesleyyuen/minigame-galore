using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace TurnBasedRPG
{
    public interface IPokemonSpeciesLoader
    {
        IEnumerable<PokemonSpecies> Load(string path);
        IUniTaskAsyncEnumerable<PokemonSpecies> LoadAsync(string path);
    }
}
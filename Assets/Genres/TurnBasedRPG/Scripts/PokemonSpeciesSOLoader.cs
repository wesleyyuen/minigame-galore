using System.Linq;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TurnBasedRPG
{
    public class PokemonSpeciesSOLoader : IPokemonSpeciesLoader
    {
        public IEnumerable<PokemonSpecies> Load(string path)
        {
            return Resources.LoadAll(path, typeof(PokemonSpecies)).Cast<PokemonSpecies>();
        }

        public async IAsyncEnumerable<PokemonSpecies> AsyncLoad(string path)
        {
            yield break;
            throw new System.NotImplementedException();
        }
    }
}
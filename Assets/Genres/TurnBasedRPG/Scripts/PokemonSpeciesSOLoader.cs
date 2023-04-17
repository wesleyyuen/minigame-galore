using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

namespace TurnBasedRPG
{
    public class PokemonSpeciesSOLoader : IPokemonSpeciesLoader
    {
        public IEnumerable<PokemonSpecies> Load(string path)
        {
            return Resources.LoadAll(path, typeof(PokemonSpecies)).Cast<PokemonSpecies>();
        }

        public IUniTaskAsyncEnumerable<PokemonSpecies> LoadAsync(string path)
        {
            return UniTaskAsyncEnumerable.Create<PokemonSpecies>(async (writer, token) => {
                await UniTask.Yield();
            });
        }
    }
}
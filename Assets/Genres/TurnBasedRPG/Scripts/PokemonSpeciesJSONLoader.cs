using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

namespace TurnBasedRPG
{
    public class PokemonSpeciesJSONLoader : IPokemonSpeciesLoader
    {
        public IEnumerable<PokemonSpecies> Load(string path)
        {
            var results = Enumerable.Empty<PokemonSpecies>();
            if (!Directory.Exists(path))
            {
                Debug.LogWarning($"Cannot find a directory at path {path}!");
                return results;
            }

            foreach (string file in Directory.EnumerateFiles(path, "*.json"))
            {
                results.Append(JsonUtility.FromJson<PokemonSpecies>(File.ReadAllText(file)));
            }
            
            return results;
        }

        public IUniTaskAsyncEnumerable<PokemonSpecies> LoadAsync(string path)
        {
            return UniTaskAsyncEnumerable.Create<PokemonSpecies>(async (writer, token) => {
                await UniTask.Yield();
            });
        }
    }
}
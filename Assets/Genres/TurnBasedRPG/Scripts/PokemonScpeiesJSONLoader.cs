using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PokemonScpeiesJSONLoader : IPokemonSpeciesLoader
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

    public async IAsyncEnumerable<PokemonSpecies> AsyncLoad(string path)
    {
        yield break;
        throw new System.NotImplementedException();
    }
}

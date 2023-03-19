using System.Collections.Generic;

public interface IPokemonSpeciesLoader
{
    IEnumerable<PokemonSpecies> Load(string path);
    IAsyncEnumerable<PokemonSpecies> AsyncLoad(string path);
}

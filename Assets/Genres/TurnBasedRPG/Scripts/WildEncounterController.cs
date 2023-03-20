using System.Linq;
using UnityEngine.Tilemaps;
using UnityEngine;

public class WildEncounterController
{
    private Tilemap _grassTilemap;
    private PokemonSpeciesManifest _pokemonManifest;

    public WildEncounterController(PokemonSpeciesManifest pokemonManifest)
    {
        _pokemonManifest = pokemonManifest;

        var tilemaps = GameObject.FindObjectsOfType<Tilemap>();
        _grassTilemap = tilemaps.FirstOrDefault(tilemap => tilemap.gameObject.name == "Grass");
    }

    public Pokemon GetWildEncountersAtPostion(Vector3 position)
    {
        if (_grassTilemap == null) {
            Debug.Log("Tilemap could not be found");
            return null;
        }
        if (_grassTilemap.HasTile(Vector3Int.FloorToInt(position)) && Random.Range(0, 101) < 15)
        {
            // random encounter for now
            return new Pokemon(_pokemonManifest.GetRandomCreatureSpecies(), 5, null);
        }

        return null;
    }
}

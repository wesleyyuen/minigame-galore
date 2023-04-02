using System.Linq;
using UnityEngine.Tilemaps;
using UnityEngine;

namespace TurnBasedRPG
{
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
            if (_grassTilemap == null)
            {
                Debug.LogError("Tilemap could not be found");
                return null;
            }

            if (!_grassTilemap.HasTile(Vector3Int.FloorToInt(position)) || Random.Range(0f, 100f) >= Constants.RANDOM_ENCOUNTER_CHANCE) return null;

            // equally random encounter for now
            return new Pokemon(_pokemonManifest.GetRandomCreatureSpecies(), 5);
        }
    }
}
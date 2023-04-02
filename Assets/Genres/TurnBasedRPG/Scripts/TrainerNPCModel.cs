using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedRPG
{
    public class TrainerNPCModel : TrainerModel
    {
        public TrainerNPCAI AI;
        [SerializeField] private List<Pokemon> _pokemons = new List<Pokemon>();

        protected override void Start()
        {
            base.Start();

            foreach (var pkmn in _pokemons)
            {
                TrainerInfo.AddCreature(new Pokemon(pkmn.Species, pkmn.Level, pkmn.Name));
            }
        }
    }
}
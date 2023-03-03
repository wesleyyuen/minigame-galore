using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerNPCModel : TrainerModel
{
    public TrainerNPCAI AI;
    [SerializeField] private List<Pokemon> _pokemons = new List<Pokemon>();

    protected override void Start()
    {
        base.Start();

        foreach (var pkmn in _pokemons)
        {
            TrainerInfo.AddCreature(new Pokemon(pkmn.Species, pkmn.Level));
        }
    }
}

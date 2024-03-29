using UnityEngine;

namespace TurnBasedRPG
{
    public abstract class TrainerNPCAI : ScriptableObject
    {
        public abstract Move GetMove(Pokemon pokemon, Pokemon target);
    }
}
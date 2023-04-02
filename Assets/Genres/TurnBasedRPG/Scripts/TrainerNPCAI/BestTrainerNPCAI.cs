using System.Linq;
using UnityEngine;

namespace TurnBasedRPG
{
    [CreateAssetMenu(fileName = "BestTrainerNPCAI", menuName = "ScriptableObjects/TurnBasedRPG/TrainerNPCAI/BestTrainerNPCAI")]
    public class BestTrainerNPCAI : TrainerNPCAI
    {
        public override Move GetMove(Pokemon pokemon, Pokemon target)
        {
            return GetFastestAndMaxDamageAttackMove(pokemon, target);
        }

        private Move GetFastestAndMaxDamageAttackMove(Pokemon pokemon, Pokemon target)
        {
            var moves = pokemon.CurrentMoves.OrderByDescending(move => move.Priority).ToList();
            Move selection = moves[0];
            float maxDamage = 0f;
            foreach (var move in moves)
            {
                if (move == null || move is not AttackMove attackMove) continue;

                float currMultiplier;
                attackMove.Type.GetDamageEffectiveness(pokemon.Species, target.Species, out currMultiplier);
                float damage = currMultiplier * attackMove.Damage;
                if (damage >= target.CurrentHP)
                {
                    return attackMove;
                }
                else if (damage > maxDamage)
                {
                    maxDamage = damage;
                    selection = attackMove;
                }
            }

            return selection;
        }
    }
}
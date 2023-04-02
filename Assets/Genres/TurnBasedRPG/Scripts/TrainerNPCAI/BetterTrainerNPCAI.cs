using UnityEngine;

namespace TurnBasedRPG
{
    [CreateAssetMenu(fileName = "BetterTrainerNPCAI", menuName = "ScriptableObjects/TurnBasedRPG/TrainerNPCAI/BetterTrainerNPCAI")]
    public class BetterTrainerNPCAI : TrainerNPCAI
    {
        public override Move GetMove(Pokemon pokemon, Pokemon target)
        {
            return GetMaxDamageAttackMove(pokemon, target);
        }

        private Move GetMaxDamageAttackMove(Pokemon pokemon, Pokemon target)
        {
            Move selection = pokemon.GetMoveByIndex(0);
            float maxDamage = 0f;
            foreach (var move in pokemon.CurrentMoves)
            {
                if (move == null || move is not AttackMove attackMove) continue;

                float currMultiplier;
                attackMove.Type.GetDamageEffectiveness(pokemon.Species, target.Species, out currMultiplier);
                float damage = currMultiplier * attackMove.Damage;
                if (damage > maxDamage)
                {
                    maxDamage = damage;
                    selection = attackMove;
                }
            }

            return selection;
        }
    }
}
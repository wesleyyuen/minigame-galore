using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BadTrainerNPCAI", menuName = "ScriptableObjects/TurnBasedRPG/TrainerNPCAI/BadTrainerNPCAI")]
public class BadTrainerNPCAI : TrainerNPCAI
{
    public override Move GetMove(Pokemon pokemon, Pokemon target)
    {
        return GetMinDamageAttackMove(pokemon, target);
    }

    private Move GetMinDamageAttackMove(Pokemon pokemon, Pokemon target)
    {
        Move selection = pokemon.GetMoveByIndex(0);
        float minDamage = 0f;
        foreach (var move in pokemon.CurrentMoves)
        {
            if (move == null || move is not AttackMove attackMove) continue;

            float currMultiplier;
            attackMove.Type.GetDamageEffectiveness(pokemon.Species, target.Species, out currMultiplier);
            float damage = currMultiplier * attackMove.Damage;
            if (damage < minDamage)
            {
                minDamage = damage;
                selection = attackMove;
            }
        }

        return selection;
    }
}

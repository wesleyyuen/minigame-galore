using System;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedRPG;
using TurnBasedRPG.UI;
using MEC;
using Effectiveness = TurnBasedRPG.Constants.DAMAGE_EFFECTIVENESS;


[CreateAssetMenu(fileName = "AttackMove", menuName = "ScriptableObjects/TurnBasedRPG/Action/AttackMove")]
public class AttackMove : Move
{
    [SerializeField] private float _damage;
    public float Damage { get {return _damage;} set {} }

    public override IEnumerator<float> DoAction(Pokemon owner, Pokemon target, Action<BattleResult> callback)
    {
        UIManager.SetBattleText($"{owner.Name} uses {name}!");

        yield return Timing.WaitForSeconds(Constants.DIALOG_DURATION);

        float multiplier;
        var effectiveness = Type.GetDamageEffectiveness(owner.Species, target.Species, out multiplier);

        target.TakeDamage(_damage * multiplier, owner);

        yield return Timing.WaitForSeconds(Constants.DIALOG_DURATION);

        if (effectiveness != Effectiveness.Neutral)
        {
            switch (effectiveness)
            {
                default: case Effectiveness.Neutral: break;
                case Effectiveness.Immune: UIManager.SetBattleText("It has no effect..."); break;
                case Effectiveness.Resist: UIManager.SetBattleText("It is not very effective..."); break;
                case Effectiveness.Super: UIManager.SetBattleText("It is super effective!"); break;
            }

            yield return Timing.WaitForSeconds(Constants.DIALOG_DURATION);
        }

        if (target.IsFainted)
        {
            UIManager.SetBattleText($"{target.Name} fainted!");
            yield return Timing.WaitForSeconds(Constants.DIALOG_DURATION);
        }

        if (owner.IsFainted)
        {
            UIManager.SetBattleText($"{owner.Name} fainted!");
            yield return Timing.WaitForSeconds(Constants.DIALOG_DURATION);
        }

        callback?.Invoke(BattleResult.Unresolved);
    }
}
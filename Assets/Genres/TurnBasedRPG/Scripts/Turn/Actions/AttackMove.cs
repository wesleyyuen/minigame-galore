using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TurnBasedRPG.UI;
using Effectiveness = TurnBasedRPG.Constants.DAMAGE_EFFECTIVENESS;

namespace TurnBasedRPG
{
    [CreateAssetMenu(fileName = "AttackMove", menuName = "ScriptableObjects/TurnBasedRPG/Action/AttackMove")]
    public class AttackMove : Move
    {
        [SerializeField] private float _damage;
        public float Damage { get {return _damage;} set {} }

        public override async UniTask<BattleResult> DoAction(Pokemon owner, Pokemon target)
        {
            await UIManager.Instance.SetBattleText($"{owner.Name} uses {name}!");

            float multiplier;
            var effectiveness = Type.GetDamageEffectiveness(owner.Species, target.Species, out multiplier);

            target.TakeDamage(_damage * multiplier, owner);

            await UniTask.Delay(TimeSpan.FromSeconds(Constants.DIALOG_DURATION));

            if (effectiveness != Effectiveness.Neutral)
            {
                switch (effectiveness)
                {
                    default: case Effectiveness.Neutral: break;
                    case Effectiveness.Immune: await UIManager.Instance.SetBattleText("It has no effect..."); break;
                    case Effectiveness.Resist: await UIManager.Instance.SetBattleText("It is not very effective..."); break;
                    case Effectiveness.Super: await UIManager.Instance.SetBattleText("It is super effective!"); break;
                }
            }

            if (target.IsFainted)
            {
                await UIManager.Instance.SetBattleText($"{target.Name} fainted!");
            }

            if (owner.IsFainted)
            {
                await UIManager.Instance.SetBattleText($"{owner.Name} fainted!");
            }

            return BattleResult.Unresolved;
        }
    }
}
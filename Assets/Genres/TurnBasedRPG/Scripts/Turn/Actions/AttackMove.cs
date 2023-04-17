using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TurnBasedRPG.UI;
using Effectiveness = TurnBasedRPG.DAMAGE_EFFECTIVENESS;

namespace TurnBasedRPG
{
    [CreateAssetMenu(fileName = "AttackMove", menuName = "ScriptableObjects/TurnBasedRPG/Action/AttackMove")]
    public class AttackMove : Move
    {
        [field: SerializeField] public float Damage { get; private set; }

        public override async UniTask<BattleResult> DoAction(Pokemon owner, Pokemon target)
        {
            await UIManager.Instance.SetBattleText( $"{owner.Name} uses {name}!");

            // accuracy check
            if (Accuracy < 1f && UnityEngine.Random.Range(0, 1) > Accuracy)
            {
                await UIManager.Instance.SetBattleText($"{owner.Name}'s attack missed!");
                return BattleResult.Unresolved;
            }

            float multiplier;
            var effectiveness = Type.GetDamageEffectiveness(owner.Species, target.Species, out multiplier);

            // critical hit check
            bool _isCriticalHit = false;
            if (UnityEngine.Random.Range(0, 1) <= Constants.CRITICAL_HIT_CHANCE_PER_STAGE)
            {
                _isCriticalHit = true;
                multiplier += Constants.CRITICAL_HIT_MULTIPLIER;
            }

            target.TakeDamage(Damage * multiplier, owner);

            await UniTask.Delay(TimeSpan.FromSeconds(Constants.DIALOG_DURATION));

            // show effectiveness text
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

            if (_isCriticalHit)
            {
                await UIManager.Instance.SetBattleText($"A critical hit!");
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
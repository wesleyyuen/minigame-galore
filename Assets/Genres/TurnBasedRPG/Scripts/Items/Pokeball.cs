using System;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using TurnBasedRPG;
using TurnBasedRPG.UI;

[CreateAssetMenu(fileName = "Pokeball", menuName = "ScriptableObjects/TurnBasedRPG/Items/Pokeball")]
public class Pokeball : Item
{
    [SerializeField] private float _catchMultiplier;
    public float CatchMultiplier => _catchMultiplier;

    public override IEnumerator<float> Use(Trainer user, Pokemon owner, Pokemon target, Action<BattleResult> callback)
    {
        if (target.Trainer != null)
        {
            UIManager.SetBattleText("You cannot catch someone's pokemon!");
            yield return Timing.WaitForSeconds(Constants.DIALOG_DURATION);
        }
        else
        {
            float targetHealthPercent = target.CurrentHP / target.Stat.HP;
            if (UnityEngine.Random.Range(0, 1) < (1 - targetHealthPercent))
            {
                user.AddCreature(target);
                UIManager.SetBattleText($"You caught {target.Name}!");
                yield return Timing.WaitForSeconds(Constants.DIALOG_DURATION);
                callback?.Invoke(BattleResult.WildPokemonCaught);
                yield break;
            }
            else
            {
                UIManager.SetBattleText($"{target.Name} broke free!");
                yield return Timing.WaitForSeconds(Constants.DIALOG_DURATION);
            }
        }

        callback?.Invoke(BattleResult.Unresolved);
    }
}

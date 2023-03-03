using System;
using System.Collections.Generic;
using MEC;
using TurnBasedRPG;
using TurnBasedRPG.UI;

public class SwitchPokemon : ITurnAction
{
    // Switching Always goes first
    public int Priority => int.MaxValue;
    private Action _uiCallback;

    public SwitchPokemon(Action uiCallback)
    {
        _uiCallback = uiCallback;
    }

    public IEnumerator<float> DoAction(Pokemon owner, Pokemon target, Action<BattleResult> callback)
    {
        if (owner != null)
        {
            UIManager.SetBattleText($"Come back, {owner.Name}!");
            yield return Timing.WaitForSeconds(Constants.DIALOG_DURATION);
        }

        UIManager.SetBattleText($"Go, {target.Name}!");

        _uiCallback?.Invoke();

        yield return Timing.WaitForSeconds(Constants.DIALOG_DURATION);

        callback?.Invoke(BattleResult.Unresolved);
    }
}
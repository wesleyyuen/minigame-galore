using System;
using System.Collections.Generic;
using MEC;
using TurnBasedRPG;
using TurnBasedRPG.UI;

public class UseItemAction : ITurnAction
{
    // Use Item goes after Switching
    public int Priority => int.MaxValue - 1;
    private Trainer _trainer;
    private Item _item;

    public UseItemAction(Trainer trainer, Item item)
    {
        _trainer = trainer;
        _item = item;
    }

    public IEnumerator<float> DoAction(Pokemon owner, Pokemon target, Action<BattleResult> callback)
    {
        // Debug.Log(_item.Name);
        UIManager.SetBattleText($"{_trainer.Name} uses {_item.Name}!");
        yield return Timing.WaitForSeconds(Constants.DIALOG_DURATION);

        var battleResult = BattleResult.Unresolved;
        var itemCoroutine = Timing.RunCoroutine(_item.Use(_trainer, owner, target, result => {
            battleResult = result;
            _trainer.ConsumeItem(_item);
        }));
        yield return Timing.WaitUntilDone(itemCoroutine);

        callback?.Invoke(battleResult);
    }
}

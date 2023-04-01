using Cysharp.Threading.Tasks;
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

    public async UniTask<BattleResult> DoAction(Pokemon owner, Pokemon target)
    {
        await UIManager.Instance.SetBattleText($"{_trainer.Name} uses {_item.Name}!");

        var battleResult = await _item.Use(_trainer, owner, target);
        _trainer.ConsumeItem(_item);

        return battleResult;
    }
}

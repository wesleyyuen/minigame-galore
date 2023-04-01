using Cysharp.Threading.Tasks;

public interface ITurnAction
{
    int Priority {get;}
    abstract UniTask<BattleResult> DoAction(Pokemon Owner, Pokemon Target);
}
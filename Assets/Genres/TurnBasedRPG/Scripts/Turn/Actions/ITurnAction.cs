using Cysharp.Threading.Tasks;

namespace TurnBasedRPG
{
    public interface ITurnAction
    {
        int Priority {get;}
        abstract UniTask<BattleResult> DoAction(Pokemon Owner, Pokemon Target);
    }
}
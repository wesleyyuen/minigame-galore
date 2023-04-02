using System;
using Cysharp.Threading.Tasks;

namespace TurnBasedRPG
{
    public class Turn : IComparable<Turn>
    {
        public Pokemon Owner;
        public Pokemon Target;
        public ITurnAction Action;

        public Turn(
            Pokemon owner,
            ITurnAction action,
            Pokemon target
        )
        {
            Owner = owner;
            Action = action;
            Target = target;
        }

        public async UniTask<BattleResult> DoAction(Action<BattleResult> callback = null)
        {
            var result = await Action.DoAction(Owner, Target);
            return result;
        }

        // Higher Priority goes first, in descending order
        public int CompareTo(Turn other)
        {
            var actionPriority = other.Action.Priority.CompareTo(Action.Priority);
            if (actionPriority == 0)
            {
                var speedPriority = other.Owner.Stat.Speed.CompareTo(Owner.Stat.Speed);
                // UnityEngine.Debug.Log($"{Owner.Name}'s {Owner.Stat.Speed} vs {other.Owner.Name}'s {other.Owner.Stat.Speed}");
                if (speedPriority == 0)
                {
                    return other.GetHashCode().CompareTo(GetHashCode());
                }
                return speedPriority;
            }
            return actionPriority;
        }
    }
}
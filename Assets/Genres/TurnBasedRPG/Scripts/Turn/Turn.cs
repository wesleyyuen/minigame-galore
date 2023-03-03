using System;
using MEC;

public class Turn : IComparable<Turn>
{
    public int Index;
    public Pokemon Owner;
    public Pokemon Target;
    public ITurnAction Action;

    public Turn(
        int index,
        Pokemon owner,
        ITurnAction action,
        Pokemon target
    )
    {
        Index = index;
        Owner = owner;
        Action = action;
        Target = target;
    }

    public CoroutineHandle DoAction(Action<BattleResult> callback)
    {
        return Timing.RunCoroutine(Action.DoAction(Owner, Target, callback));
    }

    // Higher Priority goes first
    public int CompareTo(Turn other)
    {
        var actionPriority = Action.Priority.CompareTo(other.Action.Priority);
        if (actionPriority == 0)
        {
            var speedPriority = Owner.Stat.Speed.CompareTo(other.Owner.Stat.Speed);
            if (speedPriority == 0)
            {
                return GetHashCode().CompareTo(other.GetHashCode());
            }
            return speedPriority;
        }
        return actionPriority;
    }
}
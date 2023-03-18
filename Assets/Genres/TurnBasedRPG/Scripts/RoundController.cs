using System;
using System.Collections.Generic;
using MEC;

public class RoundController
{
    private readonly PriorityQueue<Turn> _turns = new PriorityQueue<Turn>();
    private Action<BattleResult> _roundEndCallback;

    public void StartRound(IEnumerable<Turn> turns, Action<BattleResult> callback)
    {
        // Enqueue
        foreach (Turn turn in turns)
        {
            _turns.Add(turn);
        }

        _roundEndCallback = callback;

        Timing.RunCoroutine(_StartRound());
    }

    private IEnumerator<float> _StartRound()
    {
        BattleResult result = BattleResult.Unresolved;
        while (_turns.Count != 0)
        {
            // Process Turn
            Turn turn = _turns.Pop();
            yield return Timing.WaitUntilDone(turn.DoAction(r => {
                result = r;
            }));

            if (result != BattleResult.Unresolved) break;
            result = EvaluateBattleResult(turn);
            if (result != BattleResult.Unresolved || AnyPokemonFainted(turn)) break;
        }

        EndRound(result);
    }

    private BattleResult EvaluateBattleResult(Turn turn)
    {
        // Player or trainer blacked out
        if (turn.Owner?.Trainer != null && turn.Owner.Trainer.IsBlackedOut)
        {
            return turn.Owner.Trainer.IsPlayer ? BattleResult.PlayerLost : BattleResult.PlayerWon;
        }
        else if (turn.Target?.Trainer != null && turn.Target.Trainer.IsBlackedOut)
        {
            return turn.Target.Trainer.IsPlayer ? BattleResult.PlayerLost : BattleResult.PlayerWon;
        }
        // Wild Pokemon Fainted
        else if ((turn.Owner?.Trainer == null && turn.Owner.IsFainted)
              || (turn.Target?.Trainer == null && turn.Target.IsFainted))
        {
            return BattleResult.PlayerWon;
        }

        return BattleResult.Unresolved;
    }
    
    private bool AnyPokemonFainted(Turn turn)
    {
        return (turn.Owner != null && turn.Owner.IsFainted)
            || (turn.Target != null && turn.Target.IsFainted);
    }

    public void EndRound(BattleResult result)
    {
        _turns.Clear();
        _roundEndCallback?.Invoke(result);
    }
}
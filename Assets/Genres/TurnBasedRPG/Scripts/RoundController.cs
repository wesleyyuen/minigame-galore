using Cysharp.Threading.Tasks;

public class RoundController
{
    private readonly PriorityQueue<Turn> _turns = new PriorityQueue<Turn>();
    private BattleResult _result;

    public void AddTurn(Turn turn)
    {
        if (turn == null) return;
        _turns.Add(turn);
    }

    public async UniTask<BattleResult> StartRound()
    {
        _result = BattleResult.Unresolved;

        while (_turns.Count != 0)
        {
            // Process Turn
            Turn turn = _turns.Pop();
            _result = await turn.DoAction();

            if (_result != BattleResult.Unresolved) break;
            _result = EvaluateBattleResult(turn);
            if (_result != BattleResult.Unresolved || AnyPokemonFainted(turn)) break;
        }

        EndRound();

        return _result;
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

    public void EndRound()
    {
        _turns.Clear();
    }
}
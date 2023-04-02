namespace TurnBasedRPG
{
    public sealed class ExecuteTurnState : State
    {
        private BattleStateMachine _battleFsm;
            
        public ExecuteTurnState(
            BattleStateMachine stateMachine) : base(BattleStateType.ExecuteTurn.ToString())
        {
            _battleFsm = stateMachine;
        }
    
        public override async void EnterState(object args = null)
        {
            _battleFsm.BattleResult = await _battleFsm.RoundController.StartRound();

            if (_battleFsm.BattleResult == BattleResult.Unresolved)
            {
                _battleFsm.ChangeState(BattleStateType.PlayerDecision.ToString());
            }
            else
            {
                _battleFsm.ChangeState(BattleStateType.ExitBattle.ToString());
            }
        }
    }
}

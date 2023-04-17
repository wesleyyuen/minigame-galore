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

            _battleFsm.ChangeState(_battleFsm.BattleResult == BattleResult.Unresolved ?
                                   BattleStateType.PlayerDecision.ToString() :
                                   BattleStateType.ExitBattle.ToString());
        }
    }
}

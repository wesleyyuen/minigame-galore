namespace TurnBasedRPG
{
    public class BattleStateMachine : StateMachine
    {
        public RoundController RoundController { get; }
        public BattleResult BattleResult { get; set; }
        public BattleInfo BattleInfo { get; set; }
        private BattleState _parentState;
        protected override State GetInitialState() => GetState(BattleStateType.NonBattle.ToString());

        // TODO: try remove circular dependency between states and BattleStateMachine
        public BattleStateMachine(RoundController roundController)
        {
            RoundController = roundController;

            States.Add(BattleStateType.NonBattle.ToString(), new NonBattleState(this));
            States.Add(BattleStateType.EnterBattle.ToString(), new EnterBattleState(this));
            States.Add(BattleStateType.PlayerDecision.ToString(), new PlayerDecisionState(this));
            States.Add(BattleStateType.OpponentDecision.ToString(), new OpponentDecisionState(this));
            States.Add(BattleStateType.ExecuteTurn.ToString(), new ExecuteTurnState(this));
            States.Add(BattleStateType.ExitBattle.ToString(), new ExitBattleState(this));
        }
        
        public void Init(BattleState state)
        {
            _parentState = state;
        }
        
        public void StartBattle(BattleInfo battleInfo)
        {
            BattleInfo = battleInfo;
            ChangeState(BattleStateType.EnterBattle.ToString());
        }

        public void EndBattle()
        {
            _parentState.EndBattle();
        }

        public void BlackOut()
        {
            _parentState.BlackOut();
        }
    }

    public enum BattleStateType
    {
        NonBattle,
        EnterBattle,
        PlayerDecision,
        OpponentDecision,
        ExecuteTurn,
        ExitBattle
    }
}
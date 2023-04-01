using TurnBasedRPG.States;
using TurnBasedRPG.UI;

namespace TurnBasedRPG
{
    public class BattleStateMachine : StateMachine
    {
        public PlayerModel PlayerModel { get; private set; }
        public RoundController RoundController { get; private set; }
        public BattleState ParentState { get; private set;}
        public BattleResult BattleResult { get; set; }
        public BattleInfo BattleInfo { get; set; }
        protected override State GetInitialState() => GetState(BattleStateType.NonBattle.ToString());

        public BattleStateMachine(RoundController roundController)
        {
            RoundController = roundController;
        }

        public override void Initialize()
        {
            States.Add(BattleStateType.NonBattle.ToString(), new NonBattleState(this));
            States.Add(BattleStateType.EnterBattle.ToString(), new EnterBattleState(this));
            States.Add(BattleStateType.PlayerDecision.ToString(), new PlayerDecisionState(this));
            States.Add(BattleStateType.OpponentDecision.ToString(), new OpponentDecisionState(this));
            States.Add(BattleStateType.ExecuteTurn.ToString(), new ExecuteTurnState(this));
            States.Add(BattleStateType.ExitBattle.ToString(), new ExitBattleState(this));

            base.Initialize();
        }
        
        public void Init(
            BattleState state,
            PlayerModel playerModel,
            RoundController roundController)
        {
            ParentState = state;
            PlayerModel = playerModel;
            RoundController = roundController;
        }
        
        public void StartBattle(BattleInfo battleInfo)
        {
            BattleInfo = battleInfo;
            ChangeState(BattleStateType.EnterBattle.ToString());
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
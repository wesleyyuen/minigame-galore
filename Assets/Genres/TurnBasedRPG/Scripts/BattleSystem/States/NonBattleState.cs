namespace TurnBasedRPG
{
    public sealed class NonBattleState : State
    {
        public NonBattleState(BattleStateMachine stateMachine) 
            : base(BattleStateType.NonBattle.ToString())
        {
        }
    }
}

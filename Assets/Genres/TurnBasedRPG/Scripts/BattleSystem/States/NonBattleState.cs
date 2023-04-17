namespace TurnBasedRPG
{
    public sealed class NonBattleState : State
    {
        public NonBattleState() : base(BattleStateType.NonBattle.ToString())
        {
        }
    }
}

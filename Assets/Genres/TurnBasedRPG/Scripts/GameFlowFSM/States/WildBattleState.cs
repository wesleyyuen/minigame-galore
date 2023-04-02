using TurnBasedRPG.UI;

namespace TurnBasedRPG
{
    public sealed class WildBattleState : BattleState
    {
        public WildBattleState(
            GameStateMachine stateMachine,
            BattleStateMachine battleStateMachine) : base(GameState.WildBattle.ToString(), stateMachine, battleStateMachine)
        {
        }

        public override void EnterState(System.Object args = null)
        {
            if (args is not Pokemon pokemon) return;
            _battleInfo.Add(_fsm.player.TrainerInfo.GetFirstAvailablePokemon());

            _battleInfo.Add(pokemon);
            
            _fsm.player.TrainerInfo.IChooseYou();
            
            UIManager.Instance.SetPokemon(_fsm.player.TrainerInfo);
            UIManager.Instance.SetPokemon(pokemon);
            
            _battleFSM.Init(this);

            _battleFSM.StartBattle(_battleInfo);
        }
    }
}
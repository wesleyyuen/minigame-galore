using TurnBasedRPG.UI;

namespace TurnBasedRPG
{
    public sealed class TrainerBattleState : BattleState
    {
        public TrainerBattleState(
            GameStateMachine stateMachine,
            BattleStateMachine battleStateMachine) : base(GameState.TrainerBattle.ToString(), stateMachine, battleStateMachine)
        {
        }

        public override void EnterState(object args = null)
        {
            if (args is not Trainer trainer) return;
            
            _battleInfo.Add(_fsm.player.TrainerInfo.GetFirstAvailablePokemon());

            _battleInfo.Add(trainer.GetFirstAvailablePokemon());
            
            _fsm.player.TrainerInfo.IChooseYou();
            trainer.IChooseYou();
            
            UIManager.Instance.SetPokemon(_fsm.player.TrainerInfo);
            UIManager.Instance.SetPokemon(trainer);
            
            _battleFSM.Init(this);
            _battleFSM.StartBattle(_battleInfo);
        }
    }
}
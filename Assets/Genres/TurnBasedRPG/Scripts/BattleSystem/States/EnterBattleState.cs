using TurnBasedRPG.UI;

namespace TurnBasedRPG.States
{
    public sealed class EnterBattleState : State
    {
        private BattleStateMachine _battleFsm;
        
        public EnterBattleState(
            StateMachine stateMachine) : base(BattleStateType.EnterBattle.ToString(), stateMachine)
        {
            _battleFsm = (BattleStateMachine) stateMachine;
        }

        public override async void EnterState(object args = null)
        {
            _battleFsm.BattleResult = BattleResult.Unresolved;
            UIManager.Instance.SetPanelVisible(false);
            UIManager.Instance.OnBattleStart();

            var (pokemon, trainer) = _battleFsm.BattleInfo.GetOpponent(_battleFsm.PlayerModel.TrainerInfo);
            if (trainer != null)
            {
                await UIManager.Instance.SetBattleText($"You are challenged by {trainer.Name}!");
                await UIManager.Instance.SetBattleText($"{trainer.Name} sent out {pokemon.Name}!");
            }
            else
            {
                await UIManager.Instance.SetBattleText($"A wild {pokemon.Species.Name} appeared!");
            }
            
            UIManager.Instance.SetBattleText("");
            _battleFsm.ChangeState(BattleStateType.PlayerDecision.ToString());
        }

        public override void ExitState()
        {
            UIManager.Instance.SetPanelVisible(true);
        }
    }
}
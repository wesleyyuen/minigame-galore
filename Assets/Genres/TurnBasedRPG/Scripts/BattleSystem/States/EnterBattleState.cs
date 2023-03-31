using System;
using Cysharp.Threading.Tasks;
using TurnBasedRPG.UI;

namespace TurnBasedRPG.States
{
    public sealed class EnterBattleState : MonoState
    {
        private BattleStateMachine _battleFsm;
        
        public EnterBattleState(
            MonoStateMachine stateMachine) : base(BattleState.EnterBattle.ToString(), stateMachine)
        {
            _battleFsm = (BattleStateMachine) stateMachine;
        }

        public override async void EnterState(object args = null)
        {
            _battleFsm.BattleResult = BattleResult.Unresolved;
            _battleFsm.UIManager.OnBattleStart();

            var (pokemon, trainer) = _battleFsm.BattleInfo.GetOpponent(_battleFsm.PlayerModel.TrainerInfo);
            if (trainer != null)
            {
                UIManager.SetBattleText($"You are challenged by {trainer.Name}!");
                await UniTask.Delay(TimeSpan.FromSeconds(Constants.DIALOG_DURATION));
                UIManager.SetBattleText($"{trainer.Name} sent out {pokemon.Name}!");
                await UniTask.Delay(TimeSpan.FromSeconds(Constants.DIALOG_DURATION));
            }
            else
            {
                UIManager.SetBattleText($"A wild {pokemon.Species.Name} appeared!");
                await UniTask.Delay(TimeSpan.FromSeconds(Constants.DIALOG_DURATION));
            }
            
            UIManager.SetBattleText("");
            _battleFsm.ChangeState(BattleState.PlayerDecision.ToString());
        }
    }
}
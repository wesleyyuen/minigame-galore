using System.Linq;
using TurnBasedRPG.UI;

namespace TurnBasedRPG
{
    public sealed class EnterBattleState : State
    {
        private BattleStateMachine _battleFsm;
        
        public EnterBattleState(
            BattleStateMachine stateMachine) : base(BattleStateType.EnterBattle.ToString())
        {
            _battleFsm = stateMachine;
        }

        public override async void EnterState(object args = null)
        {
            _battleFsm.BattleResult = BattleResult.Unresolved;
            UIManager.Instance.SetPanelVisible(false);
            UIManager.Instance.OnBattleStart();

            var player = _battleFsm.BattleInfo.Pokemons.First(kvp => kvp.Value.IsPlayer).Value;
            var (pokemon, trainer) = _battleFsm.BattleInfo.GetOpponent(player);
            if (trainer != null)
            {
                await UIManager.Instance.SetBattleText($"You are challenged by {trainer.Name}!");
                await UIManager.Instance.SetBattleText($"{trainer.Name} sent out {pokemon.Name}!");
            }
            else
            {
                await UIManager.Instance.SetBattleText($"A wild {pokemon.Species.Name} appeared!");
            }
            
            UIManager.Instance.SetBattleTextInstantly("");
            _battleFsm.ChangeState(BattleStateType.PlayerDecision.ToString());
        }

        public override void ExitState()
        {
            UIManager.Instance.SetPanelVisible(true);
        }
    }
}
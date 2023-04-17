using System.Linq;
using TurnBasedRPG.UI;

namespace TurnBasedRPG
{
    public sealed class OpponentDecisionState : State
    {
        private BattleStateMachine _battleFsm;
        // private RoundController _roundController;
            
        // public OpponentDecisionState(
        //     BattleStateMachine stateMachine,
        //     RoundController roundController) : base(BattleStateType.OpponentDecision.ToString())
        // {
        //     _battleFsm = stateMachine;
        //     _roundController = roundController;
        // }

        public OpponentDecisionState(
            BattleStateMachine stateMachine) : base(BattleStateType.OpponentDecision.ToString())
        {
            _battleFsm = stateMachine;
        }
    
        public override void EnterState(object args = null)
        {
            var player = _battleFsm.BattleInfo.Pokemons.First(kvp => kvp.Value.IsPlayer).Value;
            var (pokemon, trainer) = _battleFsm.BattleInfo.GetOpponent(player);
            var target = player.PokemonInBattle;

            if (trainer != null && pokemon == null)
            {
                var next = trainer.GetFirstAvailablePokemon();
                trainer.IChooseYou(next);
                _battleFsm.RoundController.AddTurn(new Turn(null, new SwitchPokemon(() => {
                    UIManager.Instance.SetPokemon(trainer);
                }), next));
            }
            else
            {
                Move move = null;
                if (trainer != null && trainer.TrainerModel is TrainerNPCModel model)
                {
                    move = model.AI.GetMove(pokemon, target);
                }
                else
                {
                    move = pokemon.GetRandomMove();
                }

                _battleFsm.RoundController.AddTurn(new Turn(pokemon, move, target));
            }

            _battleFsm.ChangeState(BattleStateType.ExecuteTurn.ToString());
        }
    }
}

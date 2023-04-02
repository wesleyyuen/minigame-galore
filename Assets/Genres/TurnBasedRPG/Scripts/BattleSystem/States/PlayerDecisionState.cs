using System.Linq;
using TurnBasedRPG.UI;

namespace TurnBasedRPG
{
    public sealed class PlayerDecisionState : State
    {
        private BattleStateMachine _battleFsm;
            
        public PlayerDecisionState(
            StateMachine stateMachine) : base(BattleStateType.PlayerDecision.ToString(), stateMachine)
        {
            _battleFsm = (BattleStateMachine) stateMachine;
        }
    
        public override async void EnterState(object args = null)
        {
            var player = _battleFsm.BattleInfo.Pokemons.First(kvp => kvp.Value.IsPlayer).Value;
            var playerPkmn = player.PokemonInBattle;
            var targetKvp = _battleFsm.BattleInfo.GetOpponent(player);
            var target = targetKvp.Value?.PokemonInBattle ?? targetKvp.Key;

            // Wait for player to choose next mon
            if (playerPkmn == null)
            {
                var index = await UIManager.Instance.GetPlayerPokemonSelection();
                ChooseNextPokemon(player, null, player.GetPokemonOnHandByIndex(index));
                await _battleFsm.RoundController.StartRound();
                playerPkmn = player.PokemonInBattle;
            }

            UIManager.Instance.SetMoveText(playerPkmn);

            var action = await UIManager.Instance.GetPlayerActionSelection_1v1();
            // TODO: maybe separate each into a state
            switch (action.Type)
            {
                case TurnActionType.Fight:
                    _battleFsm.RoundController.AddTurn(new Turn(playerPkmn, playerPkmn.GetMoveByIndex(action.Index), target));
                    break;
                case TurnActionType.Pokemon:
                    ChooseNextPokemon(player, playerPkmn, player.GetPokemonOnHandByIndex(action.Index));
                    break;
                case TurnActionType.Items:
                    UIManager.Instance.OnItemSelectedFinished();
                    _battleFsm.RoundController.AddTurn(new Turn(playerPkmn, new UseItemAction(player, player.GetItem(action.Name)), target));
                    break;
                case TurnActionType.Run:
                    // TODO: handle trainer battle cannot run
                    _battleFsm.BattleResult = BattleResult.PlayerRan;
                    _battleFsm.ChangeState(BattleStateType.ExitBattle.ToString());
                    return;
                default: break;
            }

            UIManager.Instance.SetPanelVisible(false);

            _battleFsm.ChangeState(BattleStateType.OpponentDecision.ToString());
        }

        private void ChooseNextPokemon(Trainer trainer, Pokemon curr, Pokemon next)
        {
            trainer.IChooseYou(next);
            _battleFsm.RoundController.AddTurn(new Turn(curr, new SwitchPokemon(() => {
                UIManager.Instance.SetPokemon(trainer);
            }), next));
        }
    }
}

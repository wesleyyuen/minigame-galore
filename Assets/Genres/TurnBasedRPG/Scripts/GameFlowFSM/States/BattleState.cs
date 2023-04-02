using System.Linq;
using System.Collections.Generic;
using TurnBasedRPG.UI;

namespace TurnBasedRPG
{
    public class BattleState : MonoState
    {
        protected GameStateMachine _fsm;
        protected BattleInfo _battleInfo = new BattleInfo();
        protected BattleStateMachine _battleFSM;

        public BattleState(
            string name,
            GameStateMachine stateMachine,
            BattleStateMachine battleStateMachine) : base(name, stateMachine)
        {
            _fsm = stateMachine;
            _battleFSM = battleStateMachine;
        }

        public void EndBattle()
        {
            _fsm.ChangeState(GameState.Overworld.ToString());
        }

        public void BlackOut()
        {
            _fsm.ChangeState(GameState.PlayerBlackOut.ToString());
        }

        public override void ExitState()
        {
            UIManager.Instance.RemoveListener();
            _battleInfo.Clear();
        }
    }

    public class BattleInfo
    {
        // 1v1 Only for simplicity sake
        // Wild pokemon will have null trainer value
        public Dictionary<Pokemon, Trainer> Pokemons = new Dictionary<Pokemon, Trainer>();
        public bool IsTrainerBattle => Pokemons.All(kvp => kvp.Value != null);

        public void Add(Pokemon pokemon)
        {
            Pokemons.Add(pokemon, pokemon.Trainer);
        }

        public KeyValuePair<Pokemon, Trainer> GetOpponent(Trainer trainer)
        {
            return Pokemons.FirstOrDefault(kvp => kvp.Value != trainer);
        }

        public void Clear()
        {
            foreach (var (pkmn, trainer) in Pokemons)
            {
                if (trainer == null) continue;

                // reset enemy health
                if (!trainer.IsPlayer) trainer.FullHealTeam();

                trainer.IChooseYou(null);
            }

            Pokemons.Clear();
        }
    }

    public enum BattleResult
    {
        Unresolved,
        PlayerWon,
        PlayerLost,
        PlayerRan,
        WildPokemonCaught
    }
}

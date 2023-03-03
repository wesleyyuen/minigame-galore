using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace TurnBasedRPG
{
    public class GameStateMachine : StateMachine
    {
        [Header("BattleState References")]
        public CanvasGroup battleScreen;

        [Header("OverworldState References")]
        public PlayerModel player;
        public LayerMask unwalkableLayers;

        private Dictionary<GameState, State> _states = new Dictionary<GameState, State>();

        protected override State GetInitialState() => GetState(GameState.Overworld);

        [Inject]
        public void Init(
            TurnBasedRPGInput input,
            PokemonSpeciesManifest pokemonManifest,
            RoundController roundController,
            WildEncounterController wildEncounterController)
        {
            // Create all states
            _states.Add(GameState.Overworld, new OverworldState(this, input, pokemonManifest, wildEncounterController));
            _states.Add(GameState.WildBattle, new WildBattleState(this, roundController));
            _states.Add(GameState.TrainerBattle, new TrainerBattleState(this, roundController));
            _states.Add(GameState.PlayerBlackOut, new PlayerBlackOutState(this));
            _states.Add(GameState.PlayerDetected, new PlayerDetectedState(this));
        }

        public State GetState(GameState state)
        {
            return _states.FirstOrDefault(kvp => kvp.Key == state).Value;
        }
    }

    public enum GameState
    {
        Overworld,
        WildBattle,
        TrainerBattle,
        PlayerBlackOut,
        PlayerDetected
    }
}
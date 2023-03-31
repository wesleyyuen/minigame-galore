using System.Linq;
using UnityEngine;
using Zenject;

namespace TurnBasedRPG
{
    public class GameStateMachine : MonoStateMachine
    {
        [Header("BattleState References")]
        public CanvasGroup battleScreen;

        [Header("OverworldState References")]
        public PlayerModel player;
        public LayerMask unwalkableLayers;

        private SignalBus _signalBus;

        protected override MonoState GetInitialState() => GetState(GameState.Overworld.ToString());

        [Inject]
        public void Init(
            SignalBus signalBus,
            TurnBasedRPGInput input,
            PokemonSpeciesManifest pokemonManifest,
            RoundController roundController,
            WildEncounterController wildEncounterController)
        {
            _signalBus = signalBus;
            
            // Create all states
            States.Add(GameState.Overworld.ToString(), new OverworldState(this, input, pokemonManifest, wildEncounterController));
            States.Add(GameState.WildBattle.ToString(), new WildBattleState(this, roundController));
            States.Add(GameState.TrainerBattle.ToString(), new TrainerBattleState(this, roundController));
            States.Add(GameState.PlayerBlackOut.ToString(), new PlayerBlackOutState(this));
            States.Add(GameState.PlayerDetected.ToString(), new PlayerDetectedState(this));
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<PlayerDetectedSignal>(x => OnPlayerDetected(x.Detector));
        }
        
        private void OnPlayerDetected(Transform detector)
        {
            if (CurrentState != GameState.Overworld.ToString()) return;
            ChangeState(GameState.PlayerDetected.ToString(), detector);
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
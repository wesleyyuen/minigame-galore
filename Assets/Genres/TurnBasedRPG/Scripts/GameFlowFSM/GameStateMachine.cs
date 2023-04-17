using UnityEngine;
using Zenject;
using TurnBasedRPG.Input;

namespace TurnBasedRPG
{
    public class GameStateMachine : MonoStateMachine
    {
        [Header("OverworldState References")]
        public PlayerModel player;
        public LayerMask unwalkableLayers;

        protected override MonoState GetInitialState() => GetState(GameState.Overworld.ToString());

        [Inject]
        public void Init(
            SignalBus signalBus,
            TurnBasedRPGInput input,
            PokemonSpeciesManifest pokemonManifest,
            RoundController roundController,
            BattleStateMachine battleStateMachine,
            WildEncounterController wildEncounterController)
        {            
            // Create all states
            States.Add(GameState.Overworld.ToString(), new OverworldState(this, signalBus, input, wildEncounterController));
            States.Add(GameState.WildBattle.ToString(), new WildBattleState(this, battleStateMachine));
            States.Add(GameState.TrainerBattle.ToString(), new TrainerBattleState(this, battleStateMachine));
            States.Add(GameState.PlayerBlackOut.ToString(), new PlayerBlackOutState(this));
            States.Add(GameState.PlayerDetected.ToString(), new PlayerDetectedState(this));
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
using UnityEngine;
using TurnBasedRPG;

public class PlayerBlackOutState : State
{
    private GameStateMachine _fsm;

    public PlayerBlackOutState(
        GameStateMachine stateMachine) : base(stateMachine)
    {
        _fsm = stateMachine;
    }

    public override void EnterState(System.Object args = null)
    {
        // TODO: Screen White Fade In

        // TODO: Set Player position to pkmn center
        // TODO: Heal Trainer

        foreach (var pokemon in _fsm.player.TrainerInfo.PokemonsOnHand)
        {
            pokemon.Heal(pokemon.Stat.HP);
        }

        // TODO: Screen White Fade Out

        _fsm.ChangeState(_fsm.GetState(GameState.Overworld));
    }
}

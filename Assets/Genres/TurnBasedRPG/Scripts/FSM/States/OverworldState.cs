using System.Collections.Generic;
using UnityEngine;
using MEC;
using TurnBasedRPG;

public sealed class OverworldState : State
{
    private GameStateMachine _fsm;
    private TurnBasedRPGInput _input;
    private PokemonSpeciesManifest _pokemonManifest;
    private WildEncounterController _wildEncounterController;
    private Rigidbody2D _rb;
    private bool _isMoving;
    private bool _canMove = true;

    public OverworldState(
        GameStateMachine stateMachine,
        TurnBasedRPGInput input,
        PokemonSpeciesManifest pokemonManifest,
        WildEncounterController wildEncounterController) : base(GameState.Overworld.ToString(), stateMachine)
    {
        _fsm = stateMachine;
        _input = input;
        _pokemonManifest = pokemonManifest;
        _wildEncounterController = wildEncounterController;
        _rb = stateMachine.player.gameObject.GetComponent<Rigidbody2D>();
    }

    public override void UpdateState()
    {
        if (_isMoving || !_canMove) return;
        Vector3 input = _input.GetDirectionalInputVector();
        if (input == Vector3.zero) return;

        // Prioritized x-axis movements
        if (input.x > 0) Timing.RunCoroutine(_Move(Vector2.right));
        else if (input.x < 0) Timing.RunCoroutine(_Move(Vector2.left));
        else if (input.y > 0) Timing.RunCoroutine(_Move(Vector2.up));
        else if (input.y < 0) Timing.RunCoroutine(_Move(Vector2.down));
    }

    private IEnumerator<float> _Move(Vector2 dir)
    {
        if (!IsWalkable(dir)) yield break;

        Vector3 from = _rb.position;
        Vector3 to = _rb.position + dir;

        _isMoving = true;

        for (float t = 0f; t < 1f; t += Time.deltaTime / Constants.CHARACTER_MOVEMENT_DURATION) {
            _rb.position = Vector3.Lerp(from, to, t);
            yield return Timing.WaitForOneFrame;
        }

        _rb.position = to;

        _isMoving = false;

        CheckForWildEncounters(_rb.position);
    }

    private void CheckForWildEncounters(Vector2 pos)
    {
        var encounter = _wildEncounterController.GetWildEncountersAtPostion(pos);
        if (encounter != null)
        {
            _fsm.ChangeState(GameState.WildBattle.ToString(), encounter);
        }
    }

    private bool IsWalkable(Vector2 dir)
    {
        return !Physics2D.Raycast(_rb.position, dir, 0.5f, _fsm.unwalkableLayers);
    }
}
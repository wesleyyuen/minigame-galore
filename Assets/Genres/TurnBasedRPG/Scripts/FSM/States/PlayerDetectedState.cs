using System;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using TurnBasedRPG;

public sealed class PlayerDetectedState : State
{
    private GameStateMachine _fsm;
    private Rigidbody2D _trainerRB;
    private TrainerModel _trainer;

    public PlayerDetectedState(
        GameStateMachine stateMachine) : base(GameState.PlayerDetected.ToString(), stateMachine)
    {
        _fsm = stateMachine;
    }

    public override void EnterState(System.Object args = null)
    {
        var trainerTransform = (Transform) args;
        if (trainerTransform.TryGetComponent<TrainerModel>(out var trainerModel))
        {
            _trainer = trainerModel;
        }
        if (trainerTransform.TryGetComponent<Rigidbody2D>(out var rigidbody2D))
        {
            _trainerRB = rigidbody2D;
        }
        Timing.RunCoroutine(_Sequence());
    }

    private IEnumerator<float> _Sequence()
    {
        yield return Timing.WaitForSeconds(2);

        Vector3 dir = (_fsm.player.transform.position - (Vector3) _trainerRB.position).normalized;
        Vector3 from = _trainerRB.position;
        Vector3 to = _fsm.player.transform.position - dir;
        float dist = Vector3.Distance(from, to);

        for (float t = 0f; t < 1f; t += Time.deltaTime / Constants.CHARACTER_MOVEMENT_DURATION / dist) {
            _trainerRB.position = Vector3.Lerp(from, to, t);
            yield return Timing.WaitForOneFrame;
        }

        _trainerRB.position = to;

        yield return Timing.WaitForSeconds(2);

        // TODO: trainer walk to closest tile to player
        _fsm.ChangeState(GameState.TrainerBattle.ToString(), _trainer.TrainerInfo);
    }

    public override void ExitState()
    {
        _trainerRB = null;
        _trainer = null;
    }
}

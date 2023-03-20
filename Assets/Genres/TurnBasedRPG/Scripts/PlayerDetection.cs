using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedRPG;

public class PlayerDetection : MonoBehaviour
{
    private GameStateMachine _fsm;

    private void Awake()
    {
        _fsm = FindObjectOfType<GameStateMachine>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // TODO: try moving this logic to overworld state (signals maybe?)
        if (other.CompareTag("Player") && _fsm.CurrentState == _fsm.GetState(GameState.Overworld))
        {
            _fsm.ChangeState(_fsm.GetState(GameState.PlayerDetected), transform.parent);
        }
    }
}
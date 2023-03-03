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
        if (other.CompareTag("Player"))
        {
            _fsm.ChangeState(_fsm.GetState(GameState.PlayerDetected), transform.parent);
        }
    }
}
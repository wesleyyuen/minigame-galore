using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fighting;

public class IdleState : MonoState
{
    private FighterStateMachine _fsm;

    public IdleState(FighterStateMachine stateMachine) : base(GameState.Idle.ToString(), stateMachine)
    {
        _fsm = stateMachine;
    }
}

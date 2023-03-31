using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fighting;

public class BlockState : MonoState
{
    private FighterStateMachine _fsm;

    public BlockState(FighterStateMachine stateMachine) : base(GameState.Block.ToString(), stateMachine)
    {
        _fsm = stateMachine;
    }
}

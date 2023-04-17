using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Fighting;

public class BlockState : MonoState
{
    private FighterStateMachine _fsm;

    public BlockState(FighterStateMachine stateMachine) : base(FighterState.Block.ToString(), stateMachine)
    {
        _fsm = stateMachine;
    }

    public override void EnterState(object args = null)
    {
        _fsm.Input.Event_BlockCanceled += OnBlockCanceled;

        _fsm.Rigidbody.velocity = Vector2.zero;

        _fsm.Animator.SetBool("IsBlocking", true);
    }

    private void OnBlockCanceled()
    {
        _fsm.Animator.SetBool("IsBlocking", false);
        _fsm.ChangeState(FighterState.Idle.ToString());
    }

    public override void ExitState()
    {
        _fsm.Input.Event_BlockCanceled -= OnBlockCanceled;
    }
}

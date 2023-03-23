using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fighting;

public class AttackState : State
{
    private FighterStateMachine _fsm;
    private Animator _animator;
    private HitHandler _hitHandler;

    public AttackState(
        FighterStateMachine stateMachine,
        Animator animator,
        HitHandler hitHandler) : base(GameState.Attack.ToString(), stateMachine)
    {
        _fsm = stateMachine;
        _animator = animator;
        _hitHandler = hitHandler;
    }

    public override void EnterState(object args = null)
    {
        _animator.SetTrigger("Attack");
        _hitHandler.SetHitInfo(_fsm.gameObject.name, _fsm.transform.position, 10);
    }
}

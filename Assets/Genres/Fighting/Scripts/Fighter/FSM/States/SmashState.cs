using UnityEngine;
using Cysharp.Threading.Tasks;
using Fighting;

public class SmashState : MonoState
{
    private const float BASE_DAMAGE = 10f;
    private const float MAX_DAMAGE_MULTIPLIER = 4f;
    private FighterStateMachine _fsm;
    private Animator _animator;
    private HitHandler _hitHandler;

    public SmashState(
        FighterStateMachine stateMachine,
        HitHandler hitHandler) : base(FighterState.Smash.ToString(), stateMachine)
    {
        _fsm = stateMachine;
        _hitHandler = hitHandler;
    }

    public override async void EnterState(object args = null)
    {
        _fsm.Animator.SetInteger("Smash", 1);
        _fsm.Rigidbody.velocity = Vector2.zero;

        float chargedPercentage = args == null ? 0f : (float) args;
        float damage = BASE_DAMAGE * chargedPercentage.MapRange(0f, 1f, 1f, MAX_DAMAGE_MULTIPLIER);
        _hitHandler.SetHitInfo(_fsm.gameObject.name, _fsm.transform.position, damage);

        // Wait for transition
        await UniTask.WaitUntil(() => _fsm.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f);

        // Wait for completion
        await UniTask.WaitUntil(() => _fsm.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f);

        _fsm.Animator.SetInteger("Smash", 0);
        
        _fsm.ChangeState(_fsm.Input.HasSmashInput() ? FighterState.Charge.ToString() : FighterState.Idle.ToString());
    }
}

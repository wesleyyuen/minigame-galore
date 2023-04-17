using UnityEngine;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Fighting;

public class ChargeState : MonoState
{
    private const float MAX_CHARGE_DURATION = 2f;
    private FighterStateMachine _fsm;
    private CancellationTokenSource _cts = new CancellationTokenSource();

    public ChargeState(FighterStateMachine stateMachine) : base(FighterState.Charge.ToString(), stateMachine)
    {
        _fsm = stateMachine;
    }

    public override async void EnterState(object args = null)
    {
        if (_cts != null) _cts.Dispose();
        _cts = new CancellationTokenSource();

        _fsm.Input.Event_ChargeSmash += OnChargeSmash;
        
        _fsm.Rigidbody.velocity = Vector2.zero;
        
        // TODO: maybe shouldn't cancel like this?
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: _cts.Token);
            _fsm.Animator.SetBool("IsCharging", true);
        } catch (Exception) {}
    }
    
    private void OnChargeSmash(float chargedDuration)
    {
        if (Mathf.Approximately(chargedDuration, 0f))
        {
            _fsm.ChangeState(FighterState.Smash.ToString());
        }
        else
        {
            float duration = Mathf.Min(chargedDuration, MAX_CHARGE_DURATION);
            float charged_01 = duration.MapRange(0f, MAX_CHARGE_DURATION, 0f, 1f);
            _fsm.ChangeState(FighterState.Smash.ToString(), charged_01);
        }
    }

    public override void ExitState()
    {
        _cts.Cancel();
        _fsm.Animator.SetBool("IsCharging", false);
        _fsm.Input.Event_ChargeSmash -= OnChargeSmash;
    }
}

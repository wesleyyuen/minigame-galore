using System;
using UnityEngine;
using Fighting;

public abstract class FighterInput : ScriptableObject
{
    protected FighterStateMachine fsm {get; private set;}
    public void SetSelf(FighterStateMachine self) => fsm = self;

    public virtual void Update() {}

#region Movement
    public abstract Vector2 GetDirectionalInputVector();
#endregion

#region Jump
    public event Action Event_Jump;
    protected virtual void Jump() => Event_Jump?.Invoke();
    public event Action Event_JumpCanceled;
    protected virtual void CancelJump() => Event_JumpCanceled?.Invoke();
    public abstract bool HasJumpInput();
#endregion

#region Smash
    public event Action Event_Smash;
    protected virtual void Smash() => Event_Smash?.Invoke();
    public event Action<float> Event_ChargeSmash;
    protected virtual void ChargeSmash(float chargedDuration) => Event_ChargeSmash?.Invoke(chargedDuration);
    public abstract bool HasSmashInput();
#endregion

#region Block
    public event Action Event_Block;
    protected virtual void Block() => Event_Block?.Invoke();
    public event Action Event_BlockCanceled;
    protected virtual void CancelBlock() => Event_BlockCanceled?.Invoke();
    public abstract bool HasBlockInput();
#endregion
}

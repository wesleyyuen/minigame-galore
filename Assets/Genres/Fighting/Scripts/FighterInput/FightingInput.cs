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

#region Attack
    public event Action Event_Attack;
    protected virtual void Attack() => Event_Attack?.Invoke();
    public abstract bool HasAttackInput();
#endregion

#region Block
    public event Action Event_Block;
    protected virtual void Block() => Event_Block?.Invoke();
    public abstract bool HasBlockInput();
#endregion
}

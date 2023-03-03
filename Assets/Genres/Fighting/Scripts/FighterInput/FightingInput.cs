using System;
using UnityEngine;

public abstract class FighterInput : ScriptableObject
{
    public abstract Vector2 GetDirectionalInputVector();
    public abstract bool HasJumpInput();
    public abstract event Action Event_Jump;
    public abstract event Action Event_JumpCanceled;
}

using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIFighterInput", menuName = "ScriptableObjects/Fighting/FighterInput/AIFighterInput")]
public class AIFighterInput : FighterInput
{
    public override Vector2 GetDirectionalInputVector()
    {
        return new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
    }

    public override event Action Event_Jump, Event_JumpCanceled;
    public override bool HasJumpInput()
    {
        return false;
    }
}

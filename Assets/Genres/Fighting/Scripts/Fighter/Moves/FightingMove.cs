using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingMove
{
    public float Damage;
    public string AnimationTrigger;
    private bool _isPerforming;
    public bool IsPerforming => _isPerforming;
    public void OnBegin()
    {
        _isPerforming = true;
    }

    public void OnDamageBegin()
    {

    }

    public void OnDamageEnd()
    {

    }

    public void OnEnd()
    {
        _isPerforming = false;
    }
}

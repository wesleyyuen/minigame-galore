using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fighting;

public class HitHandler : MonoBehaviour
{
    private HitInfo _currentHitInfo;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == transform.parent || other.isTrigger) return;

        if (other.TryGetComponent<FighterStateMachine>(out var target))
        {
            if (_currentHitInfo != null) target.TakeDamage(_currentHitInfo);
            _currentHitInfo = null;
        }
    }

    public void SetHitInfo(string name, Vector2 origin, float damage)
    {
        _currentHitInfo = new HitInfo();
        _currentHitInfo.Name = name;
        _currentHitInfo.Origin = origin;
        _currentHitInfo.Damage = damage;
    }
}

public class HitInfo
{
    public string Name;
    public Vector2 Origin;
    public float Damage;
}
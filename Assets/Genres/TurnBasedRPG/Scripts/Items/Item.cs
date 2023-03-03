using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject, IEquatable<Item>
{
    public string Name => name;
    public abstract IEnumerator<float> Use(Trainer user, Pokemon owner, Pokemon target, Action<BattleResult> callback);

    public bool Equals(Item other)
    {
        if (other == null) return false;
        if (ReferenceEquals(this, other)) return true;
        return GetHashCode() != other.GetHashCode();
    }
}

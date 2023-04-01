using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class Item : ScriptableObject, IEquatable<Item>
{
    public string Name => name;
    public abstract UniTask<BattleResult> Use(Trainer user, Pokemon owner, Pokemon target);

    public bool Equals(Item other)
    {
        if (other == null) return false;
        if (ReferenceEquals(this, other)) return true;
        return GetHashCode() != other.GetHashCode();
    }
}

using UnityEngine;
using System.Collections.Generic;

public interface IPooledObject
{
    System.Action<GameObject> Event_Recycle {get; set;}
    void OnSpawned(Dictionary<string, object> args);
    void OnDespawned();
}

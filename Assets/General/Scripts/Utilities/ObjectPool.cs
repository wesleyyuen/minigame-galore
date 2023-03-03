using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] protected List<SpawnerObject> _spawnerObjects;
    protected Dictionary<string, Stack<GameObject>> _pools = new Dictionary<string, Stack<GameObject>>();

    protected void Awake()
    {
        for (int i = 0; i < _spawnerObjects.Count; ++i) {
            SpawnerObject so = _spawnerObjects[i];
            if (so.IsValid())
            {
                Stack<GameObject> stack = new Stack<GameObject>();

                for (int j = 0; j < so.ObjectPoolSize; ++j) {
                    GameObject obj = Instantiate(so.GameObject, transform);
                    obj.SetActive(false);
                    stack.Push(obj);
                }

                _pools[so.Id] = stack;
            }
        }
    }

    public virtual GameObject GetAnyFromPool(Vector3 position, Quaternion rotation, Dictionary<string, object> args = null, System.Action<GameObject> callback = null)
    {
        SpawnerObject so = _spawnerObjects[Random.Range(0, _spawnerObjects.Count)];
        return GetByIdFromPool(so.Id, position, rotation, args, callback);
    }

    public virtual GameObject GetByIdFromPool(string id, Vector3 position, Quaternion rotation, Dictionary<string, object> args = null, System.Action<GameObject> callback = null)
    {
        if (!_pools.ContainsKey(id)) {
            Debug.LogError($"SpawnObject {id} not found.");
            return null;
        }

        Stack<GameObject> stack = _pools[id];
        GameObject obj = stack.Pop();
  
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.transform.localScale = Vector3.one;
        obj.SetActive(true);

        if (callback != null) callback(obj);
        obj.GetComponent<IPooledObject>().OnSpawned(args);

        stack.Push(obj);

        return obj;
    }

    public virtual void Recycle(GameObject obj)
    {
        // obj.GetComponent<IPooledObject>().OnDespawned();
        obj.SetActive(false);
    }
}

[System.Serializable]
public struct SpawnerObject
{
    public string Id;
    public GameObject GameObject;
    public int ObjectPoolSize;

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(Id) && GameObject != null && GameObject.TryGetComponent<IPooledObject>(out IPooledObject beat);
    }
}

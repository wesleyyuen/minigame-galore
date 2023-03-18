using System.Linq;
using System.Collections.Generic;

public class GOAPWorldStates
{
    private Dictionary<string, int> _states {get; set;} = new Dictionary<string, int>();
    public Dictionary<string, int> States => new Dictionary<string, int>(_states);

    public bool HasState(string key) => _states.ContainsKey(key);
    private void AddState(string key, int value) => _states.TryAdd(key, value);
    public void ModifyState(string key, int value)
    {
        if (_states.ContainsKey(key)) _states[key] += value;
        else _states.Add(key, value);
    }
    public bool RemoveState(string key) => _states.Remove(key);
    public void SetState(string key, int value) => _states[key] = value;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GOAPAction : MonoBehaviour
{
	public string Name {get; set;}
	public float Cost {get; set;} = 1f;
	// TODO: Add Range as Cost
	public float Range {get; set;}
	public float Duration {get; set;}
	[SerializeField] private List<(string key, int value)> _Conditions = new List<(string, int)>();
	[SerializeField] private List<(string key, int value)> _Effects = new List<(string, int)>();
	private Dictionary<string, int> _conditions = new Dictionary<string, int>();
    public Dictionary<string, int> Conditions => new Dictionary<string, int>(_conditions);
	private Dictionary<string, int> _effects = new Dictionary<string, int>();
    public Dictionary<string, int> Effects => new Dictionary<string, int>(_effects);
	public GameObject Target;
	public GOAPWorldStates AgentBeliefs;
	public bool IsRunning {get; set;}

	private void Awake()
	{
		foreach (var c in _Conditions) _conditions.TryAdd(c.key, c.value);
		foreach (var e in _Effects) _effects.TryAdd(e.key, e.value);
	}

	public bool IsAchievableGiven(Dictionary<string, int> situation)
	{
		foreach (var c in _conditions)
		{
			if (!situation.ContainsKey(c.Key)) return false;
		}

		return true;
	}
	
	public abstract bool PreAction();
	public abstract IEnumerator DoAction();
	public abstract bool PostAction();
}
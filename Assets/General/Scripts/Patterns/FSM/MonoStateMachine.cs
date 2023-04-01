using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class MonoStateMachine : MonoBehaviour
{
    private MonoState _currentState;
    protected string CurrentState => _currentState.Name;
    protected Dictionary<string, MonoState> States { get; } = new Dictionary<string, MonoState>();
    
    protected virtual void Start()
    {
        _currentState = GetInitialState();
        _currentState?.EnterState();
    }

    protected virtual void Update()
    {
        _currentState?.UpdateState();
    }

    protected virtual void FixedUpdate()
    {
        _currentState?.FixedUpdateState();
    }

    // TODO: try to remove Object args
    public virtual void ChangeState(string newStateName, object args = null)
    {
        if (!States.TryGetValue(newStateName, out var newState))
        {
            Debug.LogError($"Cannot find {newStateName} in the available states!");
            return;
        }
        
        _currentState.ExitState();
        _currentState = newState;
        _currentState.EnterState(args);
    }
    
    protected MonoState GetState(string state)
    {
        return States.FirstOrDefault(kvp => kvp.Key == state).Value;
    }

    protected abstract MonoState GetInitialState();
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State _currentState;
    protected string CurrentState => _currentState.Name;
    protected Dictionary<string, State> States { get; } = new Dictionary<string, State>();

    private void Start()
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
    public virtual void ChangeState(string newStateName, System.Object args = null)
    {
        if (!States.TryGetValue(newStateName, out var newState)) return;
        
        _currentState.ExitState();
        _currentState = newState;
        _currentState.EnterState(args);
    }
    
    protected State GetState(string state)
    {
        return States.FirstOrDefault(kvp => kvp.Key == state).Value;
    }

    protected abstract State GetInitialState();
}
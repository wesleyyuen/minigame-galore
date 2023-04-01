using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public abstract class StateMachine : IInitializable, ITickable, IFixedTickable
{
    private State _currentState;
    protected string CurrentState => _currentState.Name;
    protected Dictionary<string, State> States { get; } = new Dictionary<string, State>();
    
    public virtual void Initialize()
    {
        _currentState = GetInitialState();
        _currentState?.EnterState();
    }

    public virtual void Tick()
    {
        _currentState?.UpdateState();
    }

    public virtual void FixedTick()
    {
        _currentState?.FixedUpdateState();
    }

    // TODO: try to remove Object args
    public virtual void ChangeState(string newStateName, System.Object args = null)
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
    
    protected State GetState(string state)
    {
        return States.FirstOrDefault(kvp => kvp.Key == state).Value;
    }

    protected abstract State GetInitialState();
}
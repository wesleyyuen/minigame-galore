using System.Collections.Generic;

public abstract class StackStateMachine : StateMachine
{
    private Stack<State> _stack = new Stack<State>();
    
    public override void Initialize()
    {
        var current = GetInitialState();
        _stack.Push(current);
        current.EnterState();
    }

    public override void Tick()
    {
        if (_stack.TryPeek(out var currentState))
        {
            currentState.UpdateState();
        }
    }

    public override void FixedTick()
    {
        if (_stack.TryPeek(out var currentState))
        {
            currentState.FixedUpdateState();
        }
    }

    // TODO: try to remove Object args
    public override void ChangeState(string newStateName, object args = null)
    {
        if (!States.TryGetValue(newStateName, out var newState)) return;
        
        PopState();
        PushState(newState, args);
    }

    // TODO: try to remove Object args
    public void PushState(State newState, object args = null)
    {
        _stack.Push(newState);
        newState.EnterState(args);
    }

    public void PopState()
    {
        if (_stack.TryPop(out State prevState))
        {
            prevState.ExitState();
        }
    }
}
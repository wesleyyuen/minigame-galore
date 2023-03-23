using System.Collections.Generic;

public abstract class StackStateMachine : StateMachine
{
    private Stack<State> _stack = new Stack<State>();
    
    private void Start()
    {
        State current = GetInitialState();
        _stack.Push(current);
        current.EnterState();
    }

    protected override void Update()
    {
        if (_stack.TryPeek(out var currentState))
        {
            currentState.UpdateState();
        }
    }

    protected override void FixedUpdate()
    {
        if (_stack.TryPeek(out var currentState))
        {
            currentState.FixedUpdateState();
        }
    }

    // TODO: try to remove Object args
    public override void ChangeState(string newStateName, System.Object args = null)
    {
        if (!States.TryGetValue(newStateName, out var newState)) return;
        
        PopState();
        PushState(newState, args);
    }

    // TODO: try to remove Object args
    public void PushState(State newState, System.Object args = null)
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
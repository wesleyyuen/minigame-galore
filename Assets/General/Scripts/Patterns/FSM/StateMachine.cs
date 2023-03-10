using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State _currentState;
    void Start()
    {
        _currentState = GetInitialState();
        if (_currentState != null) _currentState.EnterState();
    }

    protected virtual void Update()
    {
        if (_currentState != null) _currentState.UpdateState();
    }

    protected virtual void FixedUpdate()
    {
        if (_currentState != null) _currentState.FixedUpdateState();
    }

    // TODO: refactor to remove Object args
    public void ChangeState(State newState, System.Object args = null)
    {
        _currentState.ExitState();

        _currentState = newState;
        _currentState.EnterState(args);
    }

    protected abstract State GetInitialState();
}
public class State
{
    public string Name { get; }

    protected State(string name, StateMachine stateMachine)
    {
        Name = name;
        // stateMachine casted to child class
    }

    public virtual void EnterState(object args = null) {}
    public virtual void ExitState() {}
    public virtual void UpdateState() {}
    public virtual void FixedUpdateState() {}
}
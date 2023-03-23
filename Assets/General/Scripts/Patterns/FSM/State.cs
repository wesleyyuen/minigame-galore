public class State
{
    public string Name { get; }
    protected StateMachine StateMachine {get;}

    protected State(string name, StateMachine stateMachine)
    {
        Name = name;
        StateMachine = stateMachine;
    }

    public virtual void EnterState(System.Object args = null) {}
    public virtual void ExitState() {}
    public virtual void UpdateState() {}
    public virtual void FixedUpdateState() {}
}
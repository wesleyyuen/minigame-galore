public class State
{
    protected StateMachine stateMachine {get; set;}

    protected State(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void EnterState(System.Object args = null) {}
    public virtual void ExitState() {}
    public virtual void UpdateState() {}
    public virtual void FixedUpdateState() {}
}
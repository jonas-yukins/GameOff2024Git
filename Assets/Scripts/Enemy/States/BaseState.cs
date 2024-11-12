public abstract class BaseState
{
    public Enemy enemy;
    public StateMachine stateMachine;
    // instance of statemachine class

    public abstract void Enter(); // like start
    public abstract void Perform(); // like update
    public abstract void Exit(); // called on active state before moving to new state

}

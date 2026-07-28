using UnityEngine;

public class BaseState<TOwner> : IState
{
    protected TOwner Owner { get; }
    
    protected StateMachine<TOwner> StateMachine { get; }
    
    public BaseState(TOwner owner, StateMachine<TOwner> stateMachine)
    {
        Owner = owner;
        StateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        
    }

    public virtual void Tick()
    {
        
    }

    public virtual void Exit()
    {
        
    }
}

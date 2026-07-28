using System;
using System.Collections.Generic;

public class StateMachine<TOwner>
{
    private readonly TOwner _owner;

    private readonly Dictionary<Type, BaseState<TOwner>> _states = new();
    
    public BaseState<TOwner> CurrentState { get; private set; }

    public StateMachine(TOwner owner)
    {
        _owner = owner;
    }

    public void AddState(BaseState<TOwner> state)
    {
        _states[state.GetType()] = state;
    }

    public void ChangeState(BaseState<TOwner> state)
    {
        if (CurrentState != null)
        {
            CurrentState.Exit();
        }

        CurrentState = state;

        if (CurrentState != null)
        {
            CurrentState.Enter();
        }
    }
    
    public void Tick()
    {
        CurrentState?.Tick();
    }
}
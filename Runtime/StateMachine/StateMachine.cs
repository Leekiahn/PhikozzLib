using System;
using System.Collections.Generic;

namespace PhikozzLib
{
    public class StateMachine<TOwner>
    {
        private readonly Dictionary<Type, BaseState<TOwner>> _states = new();

        public BaseState<TOwner> CurrentState { get; private set; }

        public void AddState(BaseState<TOwner> state)
        {
            _states[state.GetType()] = state;
        }

        public void ChangeState<TState>() where TState : BaseState<TOwner>
        {
            if (_states.TryGetValue(typeof(TState), out var newState))
            {
                CurrentState?.Exit();
                CurrentState = newState;
                CurrentState.Enter();
            }
            else
            {
                DevLog.Warning($"[StateMachine] State '{typeof(TState).Name}' is not registered.");
            }
        }

        public void Tick()
        {
            CurrentState?.Tick();
        }
    }
}

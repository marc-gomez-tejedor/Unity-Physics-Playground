using System;
using System.Collections.Generic;

public class StateMachine
{
    readonly Dictionary<Type, IState> states = new Dictionary<Type, IState>();
    public IState current;


    public void AddState<T>(T state) where T : IState
    {
        var type = typeof(T);
        if (states.ContainsKey(type)) {
            throw new InvalidOperationException($"State {type.Name} already added."); 
        }

        states[type] = state;
    }

    public void ChangeState<T>() where T : IState
    {
        var type = typeof(T);
        if (!states.TryGetValue(type, out var state))
        {
            throw new InvalidOperationException($"State {type.Name} not found.");
        }

        current?.Exit();
        current = states[typeof(T)];
        current.Enter();
    }

    public void Update()
    {
        current?.Update();
    }

    public void FixedUpdate()
    {
        current?.FixedUpdate();
    }

    public void LateUpdate()
    {
        current?.LateUpdate();
    }
}

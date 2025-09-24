using System.Collections.Generic;

public class StateMachine<TContext>
{
    readonly Dictionary<System.Type, IState<TContext>> states = new();
    public IState<TContext> currentState { get; private set; }

    public void AddState(IState<TContext> state, TContext context)
    {
        state.Init(context);
        states[state.GetType()] = state;
    }

    public void ChangeState<T>() where T : IState<TContext>
    {
        currentState?.Exit();
        currentState = states[typeof(T)];
        currentState.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }

    public void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    public void LateUpdate()
    {
        currentState?.LateUpdate();
    }
}

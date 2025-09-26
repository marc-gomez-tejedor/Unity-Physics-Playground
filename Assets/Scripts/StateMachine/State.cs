using System.Diagnostics;

public abstract class State<TContext> : IState<TContext>
{
    protected TContext Context;

    public void Init(TContext context)
    {
        Context = context;
        OnInit();
    }
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void LateUpdate() { }
    public virtual void Exit() { }
    public abstract void AssignConfigValues();

    protected virtual void OnInit() { }
}
public abstract class State<TContext, C> : IState<TContext>
{
    public TContext Context;

    public void Init(TContext context)
    {
        Debug.Write($"type {typeof(TContext).FullName}, ctx {context}");
        Context = context;
        OnInit();
    }
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void LateUpdate() { }
    public virtual void Exit() { }
    public abstract void AssignConfigValues(C controller);

    protected virtual void OnInit() { }
}

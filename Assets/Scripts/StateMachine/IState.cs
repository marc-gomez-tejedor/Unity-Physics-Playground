public interface IState
{
    void Enter();
    void Update();
    void FixedUpdate();
    void LateUpdate();
    void Exit();
}
public interface IState<TContext> : IState
{
    void Init(TContext context);
}

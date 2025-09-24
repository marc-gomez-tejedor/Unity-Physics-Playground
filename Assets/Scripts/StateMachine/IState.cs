public interface IState<TContext>
{
    void Init(TContext context);
    void Enter();
    void Update();
    void FixedUpdate();
    void LateUpdate();
    void Exit();
    void AssignConfigValues();
}

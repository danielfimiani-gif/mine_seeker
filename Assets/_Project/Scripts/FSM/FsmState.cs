public abstract class FsmState<T>
{
    protected T Owner;

    public void Initialize(T owner)
    {
        this.Owner = owner;
        OnInitialize();
    }

    protected abstract void OnInitialize();

    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnUpdate();
}

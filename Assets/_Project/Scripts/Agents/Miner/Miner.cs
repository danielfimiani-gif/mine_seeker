using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PathNodeAgent))]
class Miner : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private MinerStats config;
    [SerializeField] private Base oreBase;

    [Header("States")]
    [SerializeField] private IdleState idleState = new IdleState();
    [SerializeField] private GoToMineState goToMineState = new GoToMineState();
    [SerializeField] private MiningState miningState = new MiningState();
    [SerializeField] private ReturnToBaseState returnToBaseState = new ReturnToBaseState();
    [SerializeField] private UnloadState unloadState = new UnloadState();


    [Header("Events")]
    public UnityEvent OnMineAssigned { get; private set; } = new UnityEvent();
    public UnityEvent OnArrivedAtMine { get; private set; } = new UnityEvent();
    public UnityEvent OnInventoryFull { get; private set; } = new UnityEvent();
    public UnityEvent OnVeinEmpty { get; private set; } = new UnityEvent();
    public UnityEvent OnArrivedAtBase { get; private set; } = new UnityEvent();
    public UnityEvent OnGoldUnloaded { get; private set; } = new UnityEvent();
    public UnityEvent OnKilled { get; private set; } = new UnityEvent();

    public MinerStats Config => config;
    public Base OreBase => oreBase;
    public bool IsAlive => Context.IsAlive;
    public MinerContext Context { get; private set; } = new MinerContext();

    private FiniteStateMachine<Miner> _fsm;

    void Awake()
    {
        Context.InitializeHP(Config.MaxHP);
        idleState.Initialize(this);
        goToMineState.Initialize(this);
        miningState.Initialize(this);
        returnToBaseState.Initialize(this);
        unloadState.Initialize(this);
    }

    void Start()
    {
        FsmState<Miner>[] states = { idleState, goToMineState, miningState, returnToBaseState, unloadState };
        UnityEvent[] events = {
            OnMineAssigned, OnArrivedAtMine, OnInventoryFull,
            OnVeinEmpty, OnArrivedAtBase, OnGoldUnloaded
        };


        _fsm = new FiniteStateMachine<Miner>(states, events, idleState);

        _fsm.ConfigureTransition(idleState, goToMineState, OnMineAssigned);
        _fsm.ConfigureTransition(goToMineState, miningState, OnArrivedAtMine);
        _fsm.ConfigureTransition(miningState, idleState, OnVeinEmpty);
        _fsm.ConfigureTransition(miningState, returnToBaseState, OnInventoryFull);
        _fsm.ConfigureTransition(returnToBaseState, unloadState, OnArrivedAtBase);
        _fsm.ConfigureTransition(unloadState, idleState, OnGoldUnloaded);
    }

    void Update()
    {
        if (IsAlive) _fsm.Update();
    }

    public void TakeDamage(float amount)
    {
        if (!IsAlive) return;

        Context.TakeDamage(amount);

        if (!IsAlive)
            OnKilled?.Invoke();
    }
}

using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PathNodeAgent))]
class Enemy : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private EnemyStats config;

    [Header("States")]
    [SerializeField] private WanderState wanderState = new WanderState();
    [SerializeField] private ChaseState chaseState = new ChaseState();
    [SerializeField] private AttackState attackState = new AttackState();

    [Header("Events")]
    public UnityEvent OnPlayerDetected { get; private set; } = new UnityEvent();
    public UnityEvent OnPlayerOutOfRange { get; private set; } = new UnityEvent();
    public UnityEvent OnTargetInAttackRange { get; private set; } = new UnityEvent();
    public UnityEvent OnTargetOutOfAttackRange { get; private set; } = new UnityEvent();
    public UnityEvent OnTargetKilled { get; private set; } = new UnityEvent();
    public UnityEvent OnTargetLost { get; private set; } = new UnityEvent();

    private FiniteStateMachine<Enemy> fsm;
    public EnemyStats Config => config;
    public Miner CurrentTarget { get; private set; }
    public Vector3 HomePoint { get; private set; }

    void Awake()
    {
        HomePoint = transform.position;
        wanderState.Initialize(this);
        chaseState.Initialize(this);
        attackState.Initialize(this);
    }

    void Start()
    {
        FsmState<Enemy>[] states = { wanderState, chaseState, attackState };
        UnityEvent[] events = {
            OnPlayerDetected, OnPlayerOutOfRange,
            OnTargetInAttackRange, OnTargetOutOfAttackRange,
            OnTargetKilled, OnTargetLost
        };

        fsm = new FiniteStateMachine<Enemy>(states, events, wanderState);

        fsm.ConfigureTransition(wanderState, chaseState, OnPlayerDetected);
        fsm.ConfigureTransition(chaseState, wanderState, OnPlayerOutOfRange);
        fsm.ConfigureTransition(chaseState, attackState, OnTargetInAttackRange);
        fsm.ConfigureTransition(attackState, chaseState, OnTargetOutOfAttackRange);
        fsm.ConfigureTransition(attackState, wanderState, OnTargetKilled);
        fsm.ConfigureTransition(attackState, wanderState, OnTargetLost);
    }

    void Update()
    {
        fsm.Update();
    }

    public void BeginPursuit(Miner target)
    {
        CurrentTarget = target;
        target.Context.AddPurser(this);
    }

    public void EndPursuit()
    {
        if (CurrentTarget != null)
        {
            CurrentTarget.Context.RemovePurser(this);
            CurrentTarget = null;
        }
    }
}

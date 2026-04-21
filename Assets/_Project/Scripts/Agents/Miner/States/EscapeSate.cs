using UnityEngine;

class EscapeSate : FsmState<Miner>
{
    private PathNodeAgent _agent;
    private Animator _animator;

    protected override void OnInitialize()
    {
        _agent = Owner.GetComponent<PathNodeAgent>();
        _animator = Owner.GetComponentInChildren<Animator>();
    }

    public override void OnEnter()
    {
        Owner.Context.ClearMine();
        _agent.MovementSpeed = Owner.Config.RunSpeed;
        _agent.Destination = Owner.OreBase.transform.position;

        _animator.SetBool("IsRunning", true);
    }

    public override void OnUpdate()
    {
        if (_agent.HasReachedDestination && !Owner.Context.HasPursers())
        {
            Owner.OnStopEscaping?.Invoke();
        }
    }

    public override void OnExit()
    {
        _animator.SetBool("IsRunning", false);
    }
}
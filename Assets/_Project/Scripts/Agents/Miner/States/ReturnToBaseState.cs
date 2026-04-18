using System;
using UnityEngine;

[Serializable]
class ReturnToBaseState : FsmState<Miner>
{
    private PathNodeAgent _agent;
    private Animator _animator;

    protected override void OnInitialize()
    {
        _agent = Owner.GetComponent<PathNodeAgent>();
        _animator = Owner.GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        _animator.SetBool("IsWalking", true);
        _agent.MovementSpeed = Owner.Config.GetWalkSpeed(Owner.Context.HasOre());
        _agent.Destination = Owner.OreBase.transform.position;
    }

    public override void OnUpdate()
    {
        if (_agent.HasReachedDestination)
            Owner.OnArrivedAtBase?.Invoke();
    }

    public override void OnExit()
    {
        _animator.SetBool("IsWalking", false);
    }
}
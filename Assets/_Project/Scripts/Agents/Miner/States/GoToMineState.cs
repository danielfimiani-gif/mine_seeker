using System;
using UnityEngine;

[Serializable]
class GoToMineState : FsmState<Miner>
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
        Vector3 minePosition = Owner.Context.CurrentMine.transform.position;
        _agent.MovementSpeed = Owner.Config.GetWalkSpeed(Owner.Context.HasOre());
        _agent.Destination = minePosition;

        _animator.SetBool("IsWalking", true);
    }

    public override void OnUpdate()
    {
        if (_agent.HasReachedDestination)
            Owner.OnArrivedAtMine?.Invoke();
    }

    public override void OnExit()
    {
        _animator.SetBool("IsWalking", false);
        Debug.Log("[Miner] Exit GoToMineState");
    }
}
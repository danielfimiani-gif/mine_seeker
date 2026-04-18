using System;
using UnityEngine;

[Serializable]
class UnloadState : FsmState<Miner>
{
    private float _timer;
    private Animator _animator;
    private PathNodeAgent _agent;

    protected override void OnInitialize()
    {
        _agent = Owner.GetComponent<PathNodeAgent>();
        _animator = Owner.GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        _timer = Owner.Config.UnloadSpeed;
        _agent.MovementSpeed = 0f;
        _animator.SetBool("IsWalking", false);
    }

    public override void OnUpdate()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
            UnloadInventory();
    }

    public override void OnExit()
    {
        Debug.Log("[Miner] Exiting Unload State");
    }

    private void UnloadInventory()
    {
        Owner.OreBase.DepositOre(Owner.Context.CurrentOre);
        Owner.Context.ResetOre();
        Owner.OnGoldUnloaded?.Invoke();
    }
}
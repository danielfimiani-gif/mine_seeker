using System;
using UnityEngine;

[Serializable]
class MiningState : FsmState<Miner>
{
    private float _timer;

    private PathNodeAgent _agent;
    private Animator _animator;

    protected override void OnInitialize()
    {
        _agent = Owner.GetComponent<PathNodeAgent>();
        _animator = Owner.GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        _timer = Owner.Config.MiningSpeed;
        _agent.MovementSpeed = 0f;
        _animator.SetBool("IsMining", true);
    }

    public override void OnUpdate()
    {
        HandleMineOre();
    }

    public override void OnExit()
    {
        _animator.SetBool("IsMining", false);
    }

    private void HandleMineOre()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            ExtractOre();
            _timer = Owner.Config.MiningSpeed;

            if (HasInventoryFull())
            {
                Owner.OnInventoryFull?.Invoke();
                return;
            }

            if (!Owner.Context.CurrentMine.HasOre)
                Owner.OnVeinEmpty?.Invoke();
        }
    }

    private bool HasInventoryFull()
    {
        return Owner.Context.CurrentOre >= Owner.Config.MaxOreCapacity;
    }

    private void ExtractOre()
    {
        float extractAmount = GetAmountToExtract();
        float extractedAmount = Owner.Context.CurrentMine.ExtractOre(extractAmount);
        Owner.Context.AddOre(extractedAmount);
    }

    private float GetAmountToExtract()
    {
        if (Owner.Context.CurrentOre + Owner.Config.OrePerHit >= Owner.Config.MaxOreCapacity)
            return Owner.Config.MaxOreCapacity - Owner.Context.CurrentOre;

        return Owner.Config.OrePerHit;
    }
}
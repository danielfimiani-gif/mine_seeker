using System;
using System.Collections;
using UnityEngine;

[Serializable]
class MiningState : FsmState<Miner>
{
    [SerializeField] private GameObject pickAxe;


    private Coroutine _mineRoutine;

    private PathNodeAgent _agent;
    private Animator _animator;

    protected override void OnInitialize()
    {
        _agent = Owner.GetComponent<PathNodeAgent>();
        _animator = Owner.GetComponentInChildren<Animator>();
    }

    public override void OnEnter()
    {
        _agent.MovementSpeed = 0f;
        _animator.SetBool("IsMining", true);
        pickAxe.SetActive(true);
        _mineRoutine = Owner.StartCoroutine(MineRoutine());
        FaceTarget();
    }

    public override void OnUpdate() { }

    public override void OnExit()
    {
        if (_mineRoutine != null)
            Owner.StopCoroutine(_mineRoutine);

        pickAxe.SetActive(false);
        _animator.SetBool("IsMining", false);
    }

    private IEnumerator MineRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Owner.Config.MiningSpeed);
            ExtractOre();

            if (HasInventoryFull())
            {
                Owner.OnInventoryFull?.Invoke();
                yield break;
            }

            if (!Owner.Context.CurrentMine.HasOre)
            {
                Owner.OnVeinEmpty?.Invoke();
                yield break;
            }
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

    private void FaceTarget()
    {
        Vector3 target = Owner.Context.CurrentMine.transform.position;
        Vector3 dir = target - Owner.transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.0001f)
            Owner.transform.rotation = Quaternion.LookRotation(dir);
    }
}
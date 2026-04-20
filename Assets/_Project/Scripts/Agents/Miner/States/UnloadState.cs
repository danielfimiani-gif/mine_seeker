using System;
using System.Collections;
using UnityEngine;

[Serializable]
class UnloadState : FsmState<Miner>
{
    private Coroutine _unloadRoutine;
    private Animator _animator;
    private PathNodeAgent _agent;

    protected override void OnInitialize()
    {
        _agent = Owner.GetComponent<PathNodeAgent>();
        _animator = Owner.GetComponentInChildren<Animator>();
    }

    public override void OnEnter()
    {
        _agent.MovementSpeed = 0f;
        _animator.SetBool("IsWalking", false);
        _unloadRoutine = Owner.StartCoroutine(UnloadRoutine());
        FaceTarget();
    }

    public override void OnUpdate() { }

    public override void OnExit()
    {
        if (_unloadRoutine != null)
            Owner.StopCoroutine(_unloadRoutine);
    }

    private IEnumerator UnloadRoutine()
    {
        while (Owner.Context.CurrentOre > 0)
        {
            yield return new WaitForSeconds(Owner.Config.UnloadSpeed);
            float chunk = Mathf.Min(Owner.Config.OrePerHit, Owner.Context.CurrentOre);
            Owner.OreBase.DepositOre(chunk);
            Owner.Context.RemoveOre(chunk);
        }

        Owner.OnGoldUnloaded?.Invoke();
    }

    private void FaceTarget()
    {
        Vector3 target = Owner.OreBase.transform.position;
        Vector3 dir = target - Owner.transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.0001f)
            Owner.transform.rotation = Quaternion.LookRotation(dir);
    }
}
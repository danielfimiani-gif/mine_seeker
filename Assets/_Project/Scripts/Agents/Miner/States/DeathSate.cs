using System;
using System.Collections;
using UnityEngine;

[Serializable]
class DeathState : FsmState<Miner>
{
    private PathNodeAgent _agent;
    private Animator _animator;
    private Coroutine _respawnRoutine;

    protected override void OnInitialize()
    {
        _agent = Owner.GetComponent<PathNodeAgent>();
        _animator = Owner.GetComponentInChildren<Animator>();

    }

    public override void OnEnter()
    {
        _agent.MovementSpeed = 0;
        _agent.Destination = null;
        _animator.SetBool("IsDead", true);
        Owner.Context.ClearMine();
        _respawnRoutine = Owner.StartCoroutine(RespawnRoutine());
    }

    public override void OnUpdate() { }

    public override void OnExit()
    {
        if (_respawnRoutine != null)
            Owner.StopCoroutine(_respawnRoutine);

        _animator.SetBool("IsDead", false);
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(Owner.Config.RespawnTime);
        Owner.Respawn();
    }
}
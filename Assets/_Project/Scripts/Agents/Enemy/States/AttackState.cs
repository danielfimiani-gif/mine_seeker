using System;
using UnityEngine;

[Serializable]
class AttackState : FsmState<Enemy>
{
    private PathNodeAgent _agent;
    private Animator _animator;
    private float _selfRadius;
    private float _targetRadius;
    private float _attackTimer;

    protected override void OnInitialize()
    {
        _agent = Owner.GetComponent<PathNodeAgent>();
        _animator = Owner.GetComponentInChildren<Animator>();
        _selfRadius = Owner.GetComponentInChildren<CapsuleCollider>().radius;
    }

    public override void OnEnter()
    {
        _agent.MovementSpeed = 0f;
        _targetRadius = Owner.CurrentTarget.GetComponentInChildren<CapsuleCollider>().radius;
        FaceTarget();
        _animator.SetBool("IsAttacking", true);
        _attackTimer = 0f;
    }

    public override void OnUpdate()
    {
        Miner target = Owner.CurrentTarget;

        if (target == null)
        {
            Owner.OnTargetLost?.Invoke();
            return;
        }

        if (!target.IsAlive)
        {
            Owner.OnTargetKilled?.Invoke();
            return;
        }

        if (IsOutOfAttackRange())
        {
            Owner.OnTargetOutOfAttackRange?.Invoke();
            return;
        }

        FaceTarget();

        _attackTimer -= Time.deltaTime;
        if (_attackTimer <= 0f)
        {
            target.TakeDamage(Owner.Config.AttackDamage);
            _attackTimer = Owner.Config.AttackInterval;
        }
    }

    public override void OnExit()
    {
        _animator.SetBool("IsAttacking", false);
    }

    private bool IsOutOfAttackRange()
    {
        float range = _selfRadius + _targetRadius + 2.0f;
        float sqr = (Owner.CurrentTarget.transform.position - Owner.transform.position).sqrMagnitude;
        return sqr > range * range;
    }

    private void FaceTarget()
    {
        Vector3 dir = Owner.CurrentTarget.transform.position - Owner.transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.0001f)
            Owner.transform.rotation = Quaternion.LookRotation(dir);
    }

}
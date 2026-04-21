using System;
using UnityEngine;

[Serializable]
class ChaseState : FsmState<Enemy>
{
    private PathNodeAgent agent;
    private float pathfindingTimer;
    private float selfRadius;
    private float targetRadius;

    protected override void OnInitialize()
    {
        agent = Owner.GetComponent<PathNodeAgent>();
        selfRadius = Owner.GetComponentInChildren<CapsuleCollider>().radius;
    }

    public override void OnEnter()
    {
        agent.MovementSpeed = Owner.Config.ChaseSpeed;
        targetRadius = Owner.CurrentTarget.GetComponentInChildren<CapsuleCollider>().radius;
        agent.Destination = GetTargetPosition();
    }

    public override void OnUpdate()
    {
        pathfindingTimer += Time.deltaTime;

        if (pathfindingTimer >= Owner.Config.PathfindingIntervals)
        {
            agent.Destination = GetTargetPosition();
            pathfindingTimer -= Owner.Config.PathfindingIntervals;
        }

        if (IsOutOfRange())
        {
            Owner.OnPlayerOutOfRange?.Invoke();
            return;
        }

        if (agent.HasReachedDestination)
            Owner.OnTargetInAttackRange?.Invoke();
    }

    public override void OnExit()
    {
        pathfindingTimer = 0f;
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 targetPos = Owner.CurrentTarget.transform.position;
        Vector3 targetDir = (targetPos - Owner.transform.position).normalized;

        return targetPos - targetDir * (selfRadius + targetRadius);
    }

    private bool IsOutOfRange()
    {
        Miner target = Owner.CurrentTarget;
        if (target == null || !target.IsAlive)
            return true;

        float sqrDistanceToTarget = (target.transform.position - Owner.transform.position).sqrMagnitude;

        return sqrDistanceToTarget > Owner.Config.LoseTrackRange * Owner.Config.LoseTrackRange;
    }
}

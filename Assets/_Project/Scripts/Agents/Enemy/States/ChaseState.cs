using System;
using UnityEngine;

[Serializable]
public class ChaseState : FsmState<Enemy>
{
    [SerializeField, Range(0f, 20f)] private float chaseSpeed = 5f;
    [SerializeField, Range(0f, 20f)] private float loseTrackRange = 5f;
    [SerializeField, Range(0f, 1f)] private float pathfindingIntervals = 0.5f;

    private PathNodeAgent agent;
    private float pathfindingTimer;
    private float selfRadius;
    private float targetRadius;

    protected override void OnInitialize()
    {
        agent = Owner.GetComponent<PathNodeAgent>();
        selfRadius = Owner.GetComponentInChildren<CapsuleCollider>().radius;
        targetRadius = Owner.AttackTarget.GetComponentInChildren<CapsuleCollider>().radius;
    }

    public override void OnEnter()
    {
        agent.MovementSpeed = chaseSpeed;
        agent.Destination = GetTargetPosition();
    }

    public override void OnUpdate()
    {
        pathfindingTimer += Time.deltaTime;

        if (pathfindingTimer >= pathfindingIntervals)
        {
            agent.Destination = GetTargetPosition();
            pathfindingTimer -= pathfindingIntervals;
        }

        if (IsOutOfRange())
            Owner.OnPlayerOutOfRange?.Invoke();
    }

    public override void OnExit()
    {
        pathfindingTimer = 0f;
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 targetDir = (Owner.AttackTarget.position - Owner.transform.position).normalized;

        return Owner.AttackTarget.position - targetDir * (selfRadius + targetRadius);
    }

    private bool IsOutOfRange()
    {
        float sqrDistanceToTarget = (Owner.AttackTarget.position - Owner.transform.position).sqrMagnitude;

        return sqrDistanceToTarget > loseTrackRange * loseTrackRange;
    }
}
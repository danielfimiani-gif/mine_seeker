using System;
using UnityEngine;

[Serializable]
class WanderState : FsmState<Enemy>
{
    private PathNodeAgent agent;
    private float currentWanderAngle = 0f;

    protected override void OnInitialize()
    {
        agent = Owner.GetComponent<PathNodeAgent>();
    }

    public override void OnEnter()
    {
        agent.MovementSpeed = Owner.Config.WanderSpeed;
        agent.Destination = GetNextDestination();
        Owner.EndPursuit();
    }

    public override void OnUpdate()
    {
        if (agent.HasReachedDestination)
            agent.Destination = GetNextDestination();

        Miner nearest = FindNearestMinerInRange();
        if (nearest != null)
        {
            Owner.BeginPursuit(nearest);
            Owner.OnPlayerDetected?.Invoke();
        }
    }

    public override void OnExit()
    {
        currentWanderAngle = 0f;
    }

    private Miner FindNearestMinerInRange()
    {
        float searchSqr = Owner.Config.SearchRadius * Owner.Config.SearchRadius;
        Miner nearest = null;
        float nearestSqrDist = float.MaxValue;

        foreach (Miner miner in GameManager.Instance.Miners)
        {
            if (!miner.IsAlive)
                continue;

            float sqrDist = (miner.transform.position - Owner.transform.position).sqrMagnitude;
            if (sqrDist > searchSqr)
                continue;

            if (sqrDist < nearestSqrDist)
            {
                nearestSqrDist = sqrDist;
                nearest = miner;
            }
        }

        return nearest;
    }

    private Vector3 GetNextDestination()
    {
        float radians = currentWanderAngle * Mathf.Deg2Rad;
        float offsetX = Mathf.Cos(radians);
        float offsetZ = Mathf.Sin(radians);

        Vector3 nextDestination = Owner.HomePoint + new Vector3(offsetX, 0f, offsetZ) * Owner.Config.CircleWanderRadius;

        currentWanderAngle += 360f / Owner.Config.CircleSearchPrecision;

        if (currentWanderAngle > 360f)
            currentWanderAngle -= 360f;

        Debug.DrawLine(Owner.HomePoint + Vector3.up, nextDestination + Vector3.up, Color.magenta, 2f);

        return nextDestination;
    }
}

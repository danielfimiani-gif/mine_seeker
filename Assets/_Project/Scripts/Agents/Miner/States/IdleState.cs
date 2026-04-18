using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class IdleState : FsmState<Miner>
{
    private PathNodeAgent _agent;

    protected override void OnInitialize()
    {
        _agent = Owner.GetComponent<PathNodeAgent>();
    }

    public override void OnEnter()
    {
        _agent.MovementSpeed = 0f;
        Owner.Context.ClearMine();
    }

    public override void OnUpdate()
    {
        SearchNearestMine();

        if (Owner.Context.CurrentMine != null)
            Owner.OnMineAssigned?.Invoke();
    }

    public override void OnExit()
    {
        Debug.Log("[Miner] Exit IdleState");
    }

    private void SearchNearestMine()
    {
        float minPathCost = float.PositiveInfinity;
        Mine nearestMine = null;

        Mine[] mines = UnityEngine.Object.FindObjectsByType<Mine>(sortMode: FindObjectsSortMode.InstanceID);
        foreach (Mine mine in mines)
        {
            if (mine.HasOre && !mine.IsOccupied)
            {
                Stack<PathNode> path = PathFindingManager.Instance.CreatePath(Owner.transform.position, mine.transform.position);
                float pathCost = PathFindingManager.Instance.GetPathCost(path);
                if (minPathCost > pathCost)
                {
                    minPathCost = pathCost;
                    nearestMine = mine;
                }
            }
        }

        if (nearestMine != null)
            Owner.Context.AssignMine(nearestMine);
    }
}
using System.Collections.Generic;
using UnityEngine;

class DijkstraStrategy : IPathFindingStrategy
{
    public PathNode GetNextOpenNode(PathFindingContext pathCtx, PathNode _)
    {
        PathNode openNode = pathCtx.OpenNodes[0];
        foreach (PathNode pathNode in pathCtx.OpenNodes)
        {
            if (pathNode.AccumulatedCost < openNode.AccumulatedCost)
                openNode = pathNode;
        }

        return openNode;
    }

    public float EvaluatePathCost(Stack<PathNode> path)
    {
        PathNode[] arr = path.ToArray();
        float totalCost = 0f;

        for (int i = 0; i < arr.Length - 1; i++)
        {
            float distance = Vector3.Distance(arr[i].Position, arr[i + 1].Position);
            totalCost += distance * arr[i + 1].CostMultiplier;
        }

        return totalCost;
    }
}
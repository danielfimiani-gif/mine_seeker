using System.Collections.Generic;
using UnityEngine;

class AStarStrategy : IPathFindingStrategy
{
    public PathNode GetNextOpenNode(PathFindingContext pathCtx, PathNode destinationNode)
    {
        PathNode openNode = pathCtx.OpenNodes[0];

        float fBest = openNode.AccumulatedCost + Vector3.Distance(openNode.Position, destinationNode.Position);

        foreach (PathNode pathNode in pathCtx.OpenNodes)
        {
            float fCandidate = pathNode.AccumulatedCost + Vector3.Distance(pathNode.Position, destinationNode.Position);

            if (fCandidate < fBest)
            {
                openNode = pathNode;
                fBest = fCandidate;
            }

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
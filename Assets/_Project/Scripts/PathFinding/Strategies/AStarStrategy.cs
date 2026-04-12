

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
}
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
}
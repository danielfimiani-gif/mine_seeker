using System.Collections.Generic;

class DepthFirstStrategy : IPathFindingStrategy
{
    public PathNode GetNextOpenNode(PathFindingContext pathCtx, PathNode _)
    {
        return pathCtx.OpenNodes[^1];
    }

    public float EvaluatePathCost(Stack<PathNode> path)
    {
        return path.Count;
    }
}
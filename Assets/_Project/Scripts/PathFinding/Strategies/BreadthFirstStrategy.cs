using System.Collections.Generic;

class BreadthFirstStrategy : IPathFindingStrategy
{
    public PathNode GetNextOpenNode(PathFindingContext pathCtx, PathNode _)
    {
        return pathCtx.OpenNodes[0];
    }

    public float EvaluatePathCost(Stack<PathNode> path)
    {
        return path.Count;
    }
}
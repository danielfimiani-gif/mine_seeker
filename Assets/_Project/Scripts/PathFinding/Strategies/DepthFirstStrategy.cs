class DepthFirstStrategy : IPathFindingStrategy
{
    public PathNode GetNextOpenNode(PathFindingContext pathCtx, PathNode _)
    {
        return pathCtx.OpenNodes[^1];
    }
}
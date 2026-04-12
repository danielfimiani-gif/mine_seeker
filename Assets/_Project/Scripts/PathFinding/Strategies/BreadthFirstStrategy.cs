class BreadthFirstStrategy : IPathFindingStrategy
{
    public PathNode GetNextOpenNode(PathFindingContext pathCtx, PathNode _)
    {
        return pathCtx.OpenNodes[0];
    }
}
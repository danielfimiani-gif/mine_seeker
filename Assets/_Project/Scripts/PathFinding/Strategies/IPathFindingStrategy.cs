public interface IPathFindingStrategy
{
    PathNode GetNextOpenNode(PathFindingContext context, PathNode destinationNode);
}
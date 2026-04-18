using System.Collections.Generic;

public interface IPathFindingStrategy
{
    PathNode GetNextOpenNode(PathFindingContext context, PathNode destinationNode);
    float EvaluatePathCost(Stack<PathNode> path);
}
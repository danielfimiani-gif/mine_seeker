using System.Collections.Generic;
using UnityEngine;

class PathFindingManager : MonoBehaviourSingleton<PathFindingManager>
{
    [SerializeField] private PathNodeGenerator pathNodeGenerator = new PathNodeGenerator();
    [SerializeField] private EPathFindingStrategy pathFindingStrategy = EPathFindingStrategy.BreadthFirst;

    private List<PathNode> _pathNodes;
    private IPathFindingStrategy _currentStrategy;

    void Start()
    {
        if (_pathNodes == null)
            GeneratePath();

        SetStrategy(pathFindingStrategy);
    }

    void OnDrawGizmos()
    {
        if (_pathNodes == null)
            return;

        Gizmos.color = Color.blue;

        foreach (PathNode pathNode in _pathNodes)
        {
            foreach (PathNode adjacentNode in pathNode.AdjacentNodes)
            {
                Gizmos.DrawLine(pathNode.Position, adjacentNode.Position);
            }
        }
    }

    [ContextMenu("Generate Path")]
    private void GeneratePath()
    {
        _pathNodes = pathNodeGenerator.GenerateNodes();
    }

    public void SetStrategy(EPathFindingStrategy pathFindingStrategy)
    {
        switch (pathFindingStrategy)
        {
            case EPathFindingStrategy.BreadthFirst:
                _currentStrategy = new BreadthFirstStrategy();
                break;
            case EPathFindingStrategy.DepthFirst:
                _currentStrategy = new DepthFirstStrategy();
                break;
            case EPathFindingStrategy.Dijkstra:
                _currentStrategy = new DijkstraStrategy();
                break;
            case EPathFindingStrategy.Astar:
                _currentStrategy = new AStarStrategy();
                break;
            default:
                Debug.Log("Invalid strategy not implemented");
                break;
        }
    }

    private PathNode FindClosestNode(Vector3 position)
    {
        PathNode closestNode = null;

        float closestSqrDistance = float.MaxValue;

        foreach (PathNode pathNode in _pathNodes)
        {
            float sqrDistance = (pathNode.Position - position).sqrMagnitude;

            if (sqrDistance < closestSqrDistance)
            {
                closestSqrDistance = sqrDistance;
                closestNode = pathNode;
            }
        }

        return closestNode;
    }

    private void ResetNodes(PathFindingContext pathCtx)
    {
        foreach (PathNode pathNode in _pathNodes)
        {
            if (pathNode.CurrentState == PathNode.State.Unreviewed)
                continue;

            pathNode.CurrentState = PathNode.State.Unreviewed;
            pathNode.Parent = null;
            pathNode.AccumulatedCost = 0f;
        }

        pathCtx.Clear();
    }

    private Stack<PathNode> GeneratePath(PathNode destinationNode)
    {
        Stack<PathNode> path = new Stack<PathNode>();

        PathNode currentNode = destinationNode;

        while (currentNode != null)
        {
            path.Push(currentNode);
            currentNode = currentNode.Parent;
        }

        return path;
    }

    public Stack<PathNode> CreatePath(Vector3 origin, Vector3 destination)
    {
        if (_pathNodes == null)
            GeneratePath();

        if (_currentStrategy == null)
            SetStrategy(pathFindingStrategy);

        PathFindingContext pathCtx = new();
        Stack<PathNode> path = null;

        PathNode originNode = FindClosestNode(origin);
        PathNode destinationNode = FindClosestNode(destination);

        pathCtx.OpenNode(originNode);

        while (pathCtx.OpenNodes.Count > 0 && path == null)
        {
            PathNode openNode = _currentStrategy.GetNextOpenNode(pathCtx, destinationNode);

            if (openNode == destinationNode)
                path = GeneratePath(destinationNode);
            else
                pathCtx.OpenAdjacentNodes(openNode);

            pathCtx.CloseNode(openNode);
        }

        ResetNodes(pathCtx);

        return path;
    }

    public float GetPathCost(Stack<PathNode> path)
    {
        if (path is null)
            return float.PositiveInfinity;
        if (path.Count < 2)
            return 0f;
        return _currentStrategy.EvaluatePathCost(path);
    }
}

using System.Collections.Generic;

public class PathFindingContext
{
    public List<PathNode> OpenNodes { get; private set; }
    public List<PathNode> ClosedNodes { get; private set; }

    public PathFindingContext()
    {
        OpenNodes = new();
        ClosedNodes = new();
    }

    public void OpenNode(PathNode node)
    {
        if (OpenNodes.Contains(node))
            return;

        node.CurrentState = PathNode.State.Open;
        OpenNodes.Add(node);
    }

    public void CloseNode(PathNode node)
    {
        if (ClosedNodes.Contains(node))
            return;

        node.CurrentState = PathNode.State.Closed;
        OpenNodes.Remove(node);
        ClosedNodes.Add(node);
    }

    public void OpenAdjacentNodes(PathNode parentNode)
    {
        foreach (PathNode pathNode in parentNode.AdjacentNodes)
        {
            if (pathNode.CurrentState != PathNode.State.Unreviewed)
                continue;

            pathNode.Parent = parentNode;

            float distance = (parentNode.Position - pathNode.Position).magnitude;
            pathNode.AccumulatedCost = parentNode.AccumulatedCost + distance * pathNode.CostMultiplier;

            OpenNode(pathNode);
        }
    }

    public void Clear()
    {
        OpenNodes?.Clear();
        ClosedNodes?.Clear();
    }
}
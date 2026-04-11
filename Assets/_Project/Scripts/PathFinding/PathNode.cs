using System.Collections.Generic;
using UnityEngine;

class PathNode
{
    public enum State
    {
        Unreviewed,
        Open,
        Closed
    }

    public List<PathNode> AdjacentNodes { get; set; } = new List<PathNode>();
    public PathNode Parent { get; set; } = null;
    public State CurrentState { get; set; } = State.Unreviewed;
    public Vector3 Position { get; set; } = Vector3.zero;
    public float CostMultiplayer { get; set; } = 1f;
    public float AccumulatedCost { get; set; } = 0f;
}
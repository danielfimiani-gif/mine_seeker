using System.Collections.Generic;
using UnityEngine;

class PathNodeAgent : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float rotationSpeed = 45f;

    private Stack<PathNode> _currentPath;
    private PathNode _targetNode;
    private Vector3? _destination;
    private readonly HashSet<TerrainZone> _activeZones = new();

    public bool HasReachedDestination { get; private set; } = false;

    public Vector3? Destination
    {
        get => _destination;
        set
        {
            _destination = value;
            if (_destination != null)
            {
                _currentPath = PathFindingManager.Instance.CreatePath(transform.position, _destination.Value);
                if (_currentPath is null)
                {
                    _destination = null;
                    HasReachedDestination = true;
                    Debug.LogWarning("Create  Path does not return a valid path");
                }
                else
                {
                    _targetNode = _currentPath.Pop();
                    HasReachedDestination = false;
                }
            }
        }
    }

    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }

    void Update()
    {
        if (Destination == null || HasReachedDestination)
            return;

        Vector3 targetPosition = _targetNode.Position;
        Vector3 diff = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(diff.normalized, transform.up);
        float maxDistanceDelta = movementSpeed * GetTerrainMultiplier() * Time.deltaTime;
        float maxDegreesDelta = rotationSpeed * Time.deltaTime;

        Vector3 updatedPosition = Vector3.MoveTowards(transform.position, targetPosition, maxDistanceDelta);
        Quaternion updatedRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDegreesDelta);

        transform.SetPositionAndRotation(updatedPosition, updatedRotation);

        if (updatedPosition == targetPosition)
        {
            if (_currentPath != null && _currentPath.Count > 0)
                _targetNode = _currentPath.Pop();
            else
                HasReachedDestination = true;
        }
    }

    public void AddActiveZone(TerrainZone zone)
    {
        _activeZones.Add(zone);
    }


    public void RemoveActiveZone(TerrainZone zone)
    {
        _activeZones.Remove(zone);
    }

    private float GetTerrainMultiplier()
    {
        float m = 1f;
        foreach (TerrainZone zone in _activeZones)
            if (zone.SpeedMultiplier < m)
                m = zone.SpeedMultiplier;
        return m;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (_targetNode == null)
            return;

        float lineThicknes = 5f;
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawLine(transform.position, _targetNode.Position, lineThicknes);

        if (_currentPath != null && _currentPath.Count > 0)
        {
            PathNode[] nodes = _currentPath.ToArray();

            UnityEditor.Handles.DrawLine(_targetNode.Position, nodes[0].Position, lineThicknes);

            for (int i = 0; i < nodes.Length - 1; i++)
                UnityEditor.Handles.DrawLine(nodes[i].Position, nodes[i + 1].Position, lineThicknes);
        }
    }
#endif
}
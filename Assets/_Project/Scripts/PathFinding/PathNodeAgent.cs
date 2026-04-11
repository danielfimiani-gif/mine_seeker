using System.Collections.Generic;
using UnityEngine;

class PathNodeAgent : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float rotationSpeed = 45f;

    private Stack<PathNode> currentPath;
    private PathNode targetNode;
    private Vector3? destination;

    public bool HasReachedDestination { get; private set; } = false;

    public Vector3? Destination
    {
        get => destination;
        set
        {
            destination = value;
            if (destination != null)
            {
                currentPath = PathFindingManager.Instance.CreatePath(transform.position, destination.Value);
                targetNode = currentPath.Pop();
                HasReachedDestination = false;
            }
        }
    }

    public float MoveventSpeed { get => movementSpeed; set => movementSpeed = value; }

    void Update()
    {
        if (Destination == null || HasReachedDestination)
            return;

        Vector3 targetPosition = targetNode.Position;
        Vector3 diff = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(diff.normalized, transform.up);
        float maxDistanceDelta = movementSpeed * Time.deltaTime;
        float maxDegreesDelta = rotationSpeed * Time.deltaTime;

        Vector3 updatedPosition = Vector3.MoveTowards(transform.position, targetPosition, maxDistanceDelta);
        Quaternion updatedRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDegreesDelta);

        transform.SetPositionAndRotation(updatedPosition, updatedRotation);

        if (diff.sqrMagnitude <= Mathf.Epsilon * Mathf.Epsilon)
        {
            if (currentPath.Count > 0)
                targetNode = currentPath.Pop();
            else
                HasReachedDestination = true;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (targetNode == null)
            return;

        float lineThicknes = 5f;
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawLine(transform.position, targetNode.Position, lineThicknes);

        if (currentPath != null && currentPath.Count > 0)
        {
            PathNode[] nodes = currentPath.ToArray();

            UnityEditor.Handles.DrawLine(targetNode.Position, nodes[0].Position, lineThicknes);

            for (int i = 0; i < nodes.Length - 1; i++)
                UnityEditor.Handles.DrawLine(nodes[i].Position, nodes[i + 1].Position, lineThicknes);
        }
    }
#endif
}
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
class TerrainZone : MonoBehaviour
{
    [SerializeField, Range(0.1f, 1.0f)] private float speedMultiplayer;
    public float SpeedMultiplier => speedMultiplayer;

    void OnTriggerEnter(Collider other)
    {
        PathNodeAgent otherAgent = other.GetComponentInParent<PathNodeAgent>();
        if (otherAgent != null)
        {
            otherAgent.AddActiveZone(this);
        }

    }

    void OnTriggerExit(Collider other)
    {
        PathNodeAgent otherAgent = other.GetComponentInParent<PathNodeAgent>();
        if (otherAgent != null)
            otherAgent.RemoveActiveZone(this);
    }
}
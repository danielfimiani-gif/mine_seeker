using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
class CameraController : MonoBehaviour
{
    [Header("Pan")]
    [SerializeField, Range(1f, 50f)] private float panSpeed = 15f;
    [SerializeField, Range(1f, 100f)] private float edgeMargin = 20f;

    [Header("Zoom")]
    [SerializeField, Range(0.5f, 10f)] private float zoomStep = 2f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 40f;

    [Header("World Bounds (XZ)")]
    [SerializeField] private Vector2 minBounds = new Vector2(-50f, -50f);
    [SerializeField] private Vector2 maxBounds = new Vector2(50f, 50f);

    private Camera _cam;

    void Awake()
    {
        _cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Mouse.current == null) return;
        HandlePan();
        HandleZoom();
    }

    private void HandlePan()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 dir = Vector2.zero;

        if (mousePos.x <= edgeMargin) dir.x = -1f;
        else if (mousePos.x >= Screen.width - edgeMargin) dir.x = 1f;

        if (mousePos.y <= edgeMargin) dir.y = -1f;
        else if (mousePos.y >= Screen.height - edgeMargin) dir.y = 1f;

        if (dir == Vector2.zero) return;

        Vector3 camForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;

        if (camForward.sqrMagnitude < 0.0001f) camForward = Vector3.forward;
        if (camRight.sqrMagnitude < 0.0001f) camRight = Vector3.right;

        Vector3 move = (camRight * dir.x + camForward * dir.y) * (panSpeed * Time.deltaTime);
        Vector3 next = transform.position + move;
        next.x = Mathf.Clamp(next.x, minBounds.x, maxBounds.x);
        next.z = Mathf.Clamp(next.z, minBounds.y, maxBounds.y);
        transform.position = next;
    }

    private void HandleZoom()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;
        if (Mathf.Approximately(scroll, 0f)) return;

        float zoomDelta = -Mathf.Sign(scroll) * zoomStep;

        if (_cam.orthographic)
            _cam.orthographicSize = Mathf.Clamp(_cam.orthographicSize + zoomDelta, minZoom, maxZoom);
        else
            transform.position += transform.forward * -zoomDelta;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.8f, 0f, 0.8f);
        Vector3 a = new Vector3(minBounds.x, 0f, minBounds.y);
        Vector3 b = new Vector3(maxBounds.x, 0f, minBounds.y);
        Vector3 c = new Vector3(maxBounds.x, 0f, maxBounds.y);
        Vector3 d = new Vector3(minBounds.x, 0f, maxBounds.y);
        Gizmos.DrawLine(a, b);
        Gizmos.DrawLine(b, c);
        Gizmos.DrawLine(c, d);
        Gizmos.DrawLine(d, a);
    }
#endif
}

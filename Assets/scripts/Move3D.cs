using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Move3D : MonoBehaviour
{
    public Transform imageTarget;       // Reference to image target (or EndSafe)
    public float dragSensitivity = 0.01f; // Tweak for drag feel
    public float snapStep = 0.5f;       // Snap to grid (optional)

    private BoxCollider boxCollider;
    private bool isDragging = false;

    private Vector3 dragStartMouse;
    private Vector3 dragStartLocal;

    void Start()
    {
        if (imageTarget == null)
        {
            Debug.LogError("ImageTarget not assigned to Move3D.");
            enabled = false;
            return;
        }

        boxCollider = GetComponent<BoxCollider>();
    }

    void OnMouseDown()
    {
        isDragging = true;
        dragStartMouse = Input.mousePosition;
        dragStartLocal = transform.localPosition;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {
        if (!isDragging) return;

        Vector3 mouseDelta = Input.mousePosition - dragStartMouse;

        // Calculate movement in local X/Z axes
        float moveX = mouseDelta.x * dragSensitivity;
        float moveZ = mouseDelta.y * dragSensitivity;

        Vector3 proposedMove = dragStartLocal + new Vector3(moveX, 0, moveZ);

        // Lock Y if needed (no vertical movement)
        proposedMove.y = dragStartLocal.y;

        // Optional: Snap to grid
        proposedMove.x = Mathf.Round(proposedMove.x / snapStep) * snapStep;
        proposedMove.z = Mathf.Round(proposedMove.z / snapStep) * snapStep;

        // Convert local to world for collision check
        Vector3 worldTarget = transform.parent.TransformPoint(proposedMove);

        if (!IsBlocked(worldTarget))
        {
            transform.localPosition = proposedMove;
        }
    }

    bool IsBlocked(Vector3 worldPos)
    {
        Vector3 halfExtents = boxCollider.size * 0.5f;
        Vector3 center = worldPos + boxCollider.center;

        Collider[] hits = Physics.OverlapBox(center, halfExtents, transform.rotation);
        foreach (var hit in hits)
        {
            if (hit.gameObject != gameObject && (hit.CompareTag("Block") || hit.CompareTag("Walls")))
            {
                return true;
            }
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        if (boxCollider == null) return;
        Gizmos.color = Color.red;
        Vector3 center = transform.position + boxCollider.center;
        Vector3 size = boxCollider.size;
        Gizmos.DrawWireCube(center, size);
    }
}

using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Move3D : MonoBehaviour
{
    public Transform imageTarget;
    public float dragSensitivity = 0.001f;
    public float gridSize = 0.1f;

    private Rigidbody rb;
    private BoxCollider boxCollider;

    private bool isDragging = false;
    private Vector3 dragStartMousePos;
    private Vector3 dragStartLocalPos;

    void Start()
    {
        if (imageTarget == null)
        {
            Debug.LogError("ImageTarget not assigned in Move3D.");
            enabled = false;
            return;
        }

        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();

        rb.useGravity = false;
        rb.isKinematic = true;

        boxCollider.isTrigger = false; // ❗ This must be false
    }

    void OnMouseDown()
    {
        isDragging = true;
        dragStartMousePos = Input.mousePosition;
        dragStartLocalPos = transform.localPosition;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {
        if (!isDragging) return;

        Vector3 mouseDelta = Input.mousePosition - dragStartMousePos;
        Vector3 right = imageTarget.right;
        Vector3 forward = imageTarget.forward;

        Vector3 moveWorld = right * mouseDelta.x * dragSensitivity + forward * mouseDelta.y * dragSensitivity;
        Vector3 targetLocalPos = dragStartLocalPos + imageTarget.InverseTransformVector(moveWorld);
        targetLocalPos.y = dragStartLocalPos.y;

        if (gridSize > 0)
        {
            targetLocalPos.x = Mathf.Round(targetLocalPos.x / gridSize) * gridSize;
            targetLocalPos.z = Mathf.Round(targetLocalPos.z / gridSize) * gridSize;
        }

        // Check if the target position will collide with anything
        Vector3 targetWorldPos = imageTarget.TransformPoint(targetLocalPos);
        if (!IsBlocked(targetWorldPos))
        {
            transform.localPosition = targetLocalPos;
        }
    }

    bool IsBlocked(Vector3 targetWorldPos)
    {
        Vector3 halfExtents = Vector3.Scale(boxCollider.size * 0.5f, transform.lossyScale);
        Vector3 center = targetWorldPos + boxCollider.center;

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
        if (boxCollider == null || imageTarget == null) return;

        Gizmos.color = Color.red;
        Vector3 center = transform.position + boxCollider.center;
        Vector3 size = Vector3.Scale(boxCollider.size, transform.lossyScale);
        Gizmos.DrawWireCube(center, size);
    }
}

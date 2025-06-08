using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Move3D : MonoBehaviour
{
    // Reference to the Vuforia Image Target or parent transform
    // Used to determine the local axes for movement and coordinate conversions
    public Transform imageTarget;

    // How sensitive the drag is to mouse movement — tweak to control speed
    public float dragSensitivity = 0.001f;

    // Optional grid snapping size; set to 0 or negative to disable snapping
    public float gridSize = 0.1f;

    private BoxCollider boxCollider;  // Collider to check collisions for this piece
    private Rigidbody rb;             // Rigidbody to control physics behavior

    private bool isDragging = false;      // Tracks whether the piece is currently being dragged
    private Vector3 dragStartMousePos;    // Mouse position on drag start (screen coords)
    private Vector3 dragStartLocalPos;    // Local position of the piece when drag started

    void Start()
    {
        // Check if imageTarget is assigned
        if (imageTarget == null)
        {
            Debug.LogError("ImageTarget not assigned in Move3D.");
            enabled = false;  // Disable script if no reference
            return;
        }

        // Cache components for later use
        boxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();

        // Ensure required components exist
        if (boxCollider == null || rb == null)
        {
            Debug.LogError("BoxCollider and Rigidbody required!");
            enabled = false;
            return;
        }

        // Setup Rigidbody to disable physics interference:
        // Set to kinematic so physics forces don't move the piece
        rb.isKinematic = true;

        // Disable gravity to prevent pieces from falling
        rb.useGravity = false;
    }

    void OnMouseDown()
    {
        // Start dragging when user clicks on piece
        isDragging = true;

        // Record the mouse position when drag starts
        dragStartMousePos = Input.mousePosition;

        // Record the piece’s local position at drag start
        dragStartLocalPos = transform.localPosition;
    }

    void OnMouseUp()
    {
        // Stop dragging when user releases mouse button
        isDragging = false;
    }

    void Update()
    {
        if (!isDragging) return; // Only process movement if dragging

        // Calculate how far the mouse has moved since drag started (in screen pixels)
        Vector3 mouseDelta = Input.mousePosition - dragStartMousePos;

        // Get the imageTarget's local right and forward directions in world space
        // These represent the local X and Z axes for movement relative to the image target
        Vector3 right = imageTarget.right;
        Vector3 forward = imageTarget.forward;

        // Calculate how much to move the piece in world space based on mouse delta
        // Mouse X moves the piece along the right vector
        // Mouse Y moves the piece along the forward vector
        Vector3 moveWorld = right * mouseDelta.x * dragSensitivity + forward * mouseDelta.y * dragSensitivity;

        // Convert the world space movement vector into local space relative to the imageTarget
        Vector3 targetLocalPos = dragStartLocalPos + imageTarget.InverseTransformVector(moveWorld);

        // Lock the Y axis to prevent vertical movement (pieces stay on same height)
        targetLocalPos.y = dragStartLocalPos.y;

        // Optional: snap to grid if gridSize > 0
        if (gridSize > 0)
        {
            // Round X and Z to nearest grid increment
            targetLocalPos.x = Mathf.Round(targetLocalPos.x / gridSize) * gridSize;
            targetLocalPos.z = Mathf.Round(targetLocalPos.z / gridSize) * gridSize;
        }

        // Convert the target local position back to world space for collision checking
        Vector3 targetWorldPos = imageTarget.TransformPoint(targetLocalPos);

        // Check if this new position would collide with other blocks or walls
        if (!IsBlocked(targetWorldPos))
        {
            // If no collision, update piece’s local position to the target
            transform.localPosition = targetLocalPos;
        }
        // If blocked, piece stays where it is (no movement)
    }

    // Checks if moving the piece to targetWorldPos would collide with another block or wall
    bool IsBlocked(Vector3 targetWorldPos)
    {
        // Calculate half the size of the collider box to define overlap area
        Vector3 halfExtents = boxCollider.size * 0.5f;

        // Calculate center point of the box collider at the target position
        Vector3 center = targetWorldPos + boxCollider.center;

        // Perform an overlap box physics check to find all colliders intersecting that area
        Collider[] hits = Physics.OverlapBox(center, halfExtents, transform.rotation);

        // Loop through all colliders found in that space
        foreach (var hit in hits)
        {
            // Ignore self, but if another object with tag "Block" or "Walls" is there, it blocks the move
            if (hit.gameObject != gameObject && (hit.CompareTag("Block") || hit.CompareTag("Walls")))
            {
                return true; // Collision detected, movement blocked
            }
        }

        return false; // No collisions detected, movement allowed
    }

    // Draws a wireframe cube in the editor to visualize the collider bounds
    void OnDrawGizmosSelected()
    {
        if (boxCollider == null || imageTarget == null) return;

        Gizmos.color = Color.red;

        // Calculate world position of the collider center
        Vector3 center = transform.position + boxCollider.center;

        // Get collider size
        Vector3 size = boxCollider.size;

        // Draw wireframe box representing the collider bounds
        Gizmos.DrawWireCube(center, size);
    }
}

using UnityEngine;

public class BallGravity : MonoBehaviour
{
    public Transform imageTarget;                // Reference to the image target
    public float heightAboveMaze = 0.01f;        // Distance above the maze surface to snap the ball
    public float maxAllowedDistance = 0.05f;     // If further than this, reposition it
    private Vector3 localOffsetToImageTarget;

    void Start()
    {
        if (imageTarget == null)
        {
            enabled = false;
            return;
        }

        localOffsetToImageTarget = imageTarget.InverseTransformPoint(transform.position);

        // Disable physics-related components
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (imageTarget == null || !imageTarget.gameObject.activeInHierarchy) return;

        // Calculate expected position on the maze
        Vector3 expectedWorldPos = imageTarget.TransformPoint(localOffsetToImageTarget);

        // Check distance from current ball position to maze surface
        float distance = Vector3.Distance(transform.position, expectedWorldPos);

        if (distance > maxAllowedDistance)
        {
            // Too far — reposition it just above the maze
            Vector3 aboveMazePos = expectedWorldPos + imageTarget.up * heightAboveMaze;
            transform.position = aboveMazePos;
        }
    }
}

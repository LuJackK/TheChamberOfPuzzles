using UnityEngine;
using Vuforia;

[RequireComponent(typeof(Rigidbody))]
public class BallGravity : MonoBehaviour
{
    public Transform imageTarget;             // The Image Target or parent that defines local gravity direction
    public float gravityScale = 9.81f;        // Gravity strength

    private Rigidbody rb;
    private bool isTracked = false;           // Whether the Image Target is currently tracked

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (imageTarget == null)
        {
            Debug.LogError("Image Target not assigned to BallGravity script.");
            enabled = false;
            return;
        }

        // Subscribe to the tracking status updates of the image target
        ObserverBehaviour observer = imageTarget.GetComponent<ObserverBehaviour>();
        if (observer != null)
        {
            observer.OnTargetStatusChanged += OnTargetStatusChanged;
        }
        else
        {
            Debug.LogError("ObserverBehaviour component not found on the assigned Image Target.");
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from tracking events
        if (imageTarget != null)
        {
            ObserverBehaviour observer = imageTarget.GetComponent<ObserverBehaviour>();
            if (observer != null)
            {
                observer.OnTargetStatusChanged -= OnTargetStatusChanged;
            }
        }
    }

    // Called when tracking state of the target changes
    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        isTracked = status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED;
    }

    void FixedUpdate()
    {
        if (!isTracked) return;

        // Simulate gravity based on the "up" direction of the image target
        Vector3 simulatedGravity = -imageTarget.up * gravityScale;
        rb.AddForce(simulatedGravity, ForceMode.Acceleration);
    }
}
    
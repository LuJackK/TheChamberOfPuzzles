using UnityEngine;
using Vuforia;

public class BallGravity : MonoBehaviour
{
    public Transform imageTarget;
    public float gravityScale = 9.81f;

    private Rigidbody rb;
    private bool isTracked = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (imageTarget == null)
        {
            Debug.LogError("Image Target not assigned to BallGravity script.");
            enabled = false;
            return;
        }

        // Subscribe to tracking status updates
        var observer = imageTarget.GetComponent<ObserverBehaviour>();
        if (observer != null)
        {
            observer.OnTargetStatusChanged += OnTargetStatusChanged;
        }
        else
        {
            Debug.LogError("ObserverBehaviour not found on Image Target.");
        }
    }

    void OnDestroy()
    {
        if (imageTarget != null)
        {
            var observer = imageTarget.GetComponent<ObserverBehaviour>();
            if (observer != null)
            {
                observer.OnTargetStatusChanged -= OnTargetStatusChanged;
            }
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        isTracked = status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED;
    }

    void FixedUpdate()
    {
        if (!isTracked) return;

        Vector3 simulatedGravity = -imageTarget.up * gravityScale;
        rb.AddForce(simulatedGravity, ForceMode.Acceleration);
    }
}

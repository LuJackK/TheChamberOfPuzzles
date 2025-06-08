using UnityEngine;

public class SlidingBlock : MonoBehaviour
{
    public Vector3 targetPosition;
    public float moveSpeed = 5f;
    private bool isMoving = false;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                transform.position = targetPosition;
                isMoving = false;
                Debug.Log(gameObject.name + " reached target position.");
            }
        }
    }

    public void Move(Vector3 direction)
    {
        if (isMoving) return; // Don't interrupt current move

        Vector3 newPosition = targetPosition + direction;
        Debug.Log(gameObject.name + " trying to move to: " + newPosition);

        if (!IsBlocked(newPosition))
        {
            Debug.Log(gameObject.name + " is NOT blocked, moving.");
            targetPosition = newPosition;
            isMoving = true;
        }
        else
        {
            Debug.Log(gameObject.name + " is BLOCKED, not moving.");
        }
    }

    private bool IsBlocked(Vector3 position)
    {
        // Use a slightly smaller overlap box to avoid false collisions due to floating-point rounding
        Vector3 halfExtents = (transform.localScale * 0.5f) - Vector3.one * 0.001f;

        Collider[] colliders = Physics.OverlapBox(position, halfExtents, Quaternion.identity);
        Debug.Log("Checking block at: " + position + ", found " + colliders.Length + " colliders.");

        foreach (Collider c in colliders)
        {
            if (c.gameObject != this.gameObject && c.CompareTag("Block"))
            {
                Debug.Log("Blocked by: " + c.gameObject.name);
                return true;
            }
        }

        return false;
    }

    // Optional: Visualize the check in the Scene view (Editor only)
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        Vector3 halfExtents = (transform.localScale * 0.5f) - Vector3.one * 0.001f;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(targetPosition, halfExtents * 2f);
    }
#endif
}

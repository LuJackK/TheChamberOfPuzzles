using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    [HideInInspector]
    public bool isTouching = false;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Block") && other.gameObject != gameObject)
        {
            isTouching = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Block") && other.gameObject != gameObject)
        {
            isTouching = false;
        }
    }
}

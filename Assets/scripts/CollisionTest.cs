using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Block" || other.gameObject.tag == "Walls")
        {
            Debug.Log("Collision detected with: " + other.gameObject.name);
        }
    }
}

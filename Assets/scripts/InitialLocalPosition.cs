using UnityEngine;

public class InitialLocalPosition : MonoBehaviour
{
    [HideInInspector]
    public Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }
}

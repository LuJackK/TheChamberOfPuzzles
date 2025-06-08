using UnityEngine;

public class InputManage : MonoBehaviour
{
    public Camera cam;
    public float moveStep = 0.01f; // Match your grid spacing

    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
            if (cam == null)
            {
                Debug.LogError("No MainCamera found! Assign one to 'cam' in the Inspector.");
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick(Input.mousePosition);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            HandleClick(Input.GetTouch(0).position);
        }
    }

    void HandleClick(Vector2 screenPosition)
    {
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            SlidingBlock block = hit.collider.GetComponent<SlidingBlock>();
            if (block != null)
            {
                Vector3[] directions = {
                    new Vector3(moveStep, 0, 0),
                    new Vector3(-moveStep, 0, 0),
                    new Vector3(0, 0, moveStep),
                    new Vector3(0, 0, -moveStep)
                };

                foreach (Vector3 dir in directions)
                {
                    block.Move(dir);

                    if (block.transform.position != block.targetPosition)
                    {
                        Debug.Log("Moved " + hit.collider.name + " in direction: " + dir);
                        break;
                    }
                }
            }
        }
    }
}

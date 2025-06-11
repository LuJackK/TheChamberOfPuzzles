using UnityEngine;

public class MazeFinsihedChecker : MonoBehaviour
{
    public GameObject ball;    // Reference to TheSnitch
    public GameObject finish;  // Reference to Finish trigger

    public float finishDistance = 0.00005f; // Distance to consider as 'reached'
    public bool isBallFinished = false;
    void Update()
    {
        float distance = Vector3.Distance(ball.transform.position, finish.transform.position);

        if (distance < finishDistance)
        {
            Debug.Log("Maze completed!");
            isBallFinished = true;
        }
   
    }
}

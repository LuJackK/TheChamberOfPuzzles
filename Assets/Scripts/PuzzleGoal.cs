using UnityEngine;

public class PuzzleGoal : MonoBehaviour
{
    public bool puzzleComplete = false;

    void OnTriggerEnter(Collider other)
    {
        if (puzzleComplete) return;

        if (other.CompareTag("redPiece") && other.gameObject != gameObject)
        {
            puzzleComplete = true;
            Debug.Log("Puzzle Completed!");

            // Optional: trigger animation, sound, etc.
            // e.g., GetComponent<Animator>().SetTrigger("Open");
        }
    }
}

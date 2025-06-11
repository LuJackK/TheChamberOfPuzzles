using UnityEngine;

public class PicturePuzzle : MonoBehaviour
{
    public GameObject piece1;
    public GameObject piece2;
    public GameObject piece3;
    public GameObject piece4;
    public GameObject piece5;
    public GameObject piece6;
    public GameObject final;

    public Transform referenceTransform; // Assign this to your ImageTarget or EndSafe in Inspector
    public bool isPuzzleFinished = false;

    public float positionTolerance = 0.05f;
    public float rotationTolerance = 5f; // degrees

    void Update()
    {
        isPuzzleFinished = CheckAllRotationsEqual() && CheckPiecePositions();
        if (isPuzzleFinished)
        {
            final.GetComponent<Renderer>().enabled = false;
        }
    }

    private bool CheckAllRotationsEqual()
    {
        Quaternion reference = piece1.transform.rotation;

        return Quaternion.Angle(reference, piece2.transform.rotation) < rotationTolerance &&
               Quaternion.Angle(reference, piece3.transform.rotation) < rotationTolerance &&
               Quaternion.Angle(reference, piece4.transform.rotation) < rotationTolerance &&
               Quaternion.Angle(reference, piece5.transform.rotation) < rotationTolerance &&
               Quaternion.Angle(reference, piece6.transform.rotation) < rotationTolerance;
    }

    private bool CheckPiecePositions()
    {
        // Convert world positions into reference space
        Vector3 pos1 = referenceTransform.InverseTransformPoint(piece1.transform.position);
        Vector3 pos2 = referenceTransform.InverseTransformPoint(piece2.transform.position);
        Vector3 pos3 = referenceTransform.InverseTransformPoint(piece3.transform.position);
        Vector3 pos4 = referenceTransform.InverseTransformPoint(piece4.transform.position);
        Vector3 pos5 = referenceTransform.InverseTransformPoint(piece5.transform.position);
        Vector3 pos6 = referenceTransform.InverseTransformPoint(piece6.transform.position);

        
        // Now all positions are relative to the same local space (ImageTarget)
        return ArePositionsNear(pos2, pos1 + Vector3.right) && // 2 is right of 1
               ArePositionsNear(pos3, pos1 + Vector3.back) &&  // 3 is below 1
               ArePositionsNear(pos1, pos2 + Vector3.left) &&  // 1 is left of 2
               ArePositionsNear(pos4, pos2 + Vector3.back) &&  // 4 is below 2
               ArePositionsNear(pos1, pos3 + Vector3.forward) && // 1 is above 3
               ArePositionsNear(pos5, pos4 + Vector3.back) &&  // 5 is below 4
               ArePositionsNear(pos2, pos4 + Vector3.up) &&    // 2 is above 4
               ArePositionsNear(pos3, pos4 + Vector3.left) &&  // 3 is left of 4
               ArePositionsNear(pos6, pos5 + Vector3.right) && // 6 is right of 5
               ArePositionsNear(pos4, pos6 + Vector3.forward) && // 4 is above 6
               ArePositionsNear(pos5, pos6 + Vector3.left) &&  // 5 is left of 6
               ArePositionsNear(pos3, pos5 + Vector3.forward); // 3 is above 5
    }

    private bool ArePositionsNear(Vector3 a, Vector3 b)
    {
        float dist = Vector3.Distance(a, b);
        Debug.Log("Relative distance: " + dist);
        return dist < positionTolerance;
    }
}

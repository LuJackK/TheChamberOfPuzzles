using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public Animator DrawerAnimator;
    public Animator VaultDoorAnimator;

    private bool cubePulled = false;
    private bool mazeSolved = false;
    private bool picturePuzzleSolved = false; 

    public MazeFinsihedChecker mazeController;

    public void OnCubePulled()
    {
        if (cubePulled)
        {
            DrawerAnimator.Play("DrawerOpen");
        }
    }

    private void CheckAllPuzzles()
    {
        if (mazeController.isBallFinished && picturePuzzleSolved && cubePulled)
        {
            VaultDoorAnimator.Play("VaultOpen");
        }
    }

}
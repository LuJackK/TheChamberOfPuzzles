using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public Animator DrawerAnimator;
    public Animator VaultDoorAnimator;

    private bool picturePuzzleSolved = false;
    private bool letters1Colored = false;
    private bool letters2Colored = false;
    private bool letters3Colored = false;

    public MazeFinsihedChecker mazeController;
    public PuzzleGoal puzzleGoal;

    public Material glowingMaterial;

    public Renderer letterA;
    public Renderer letterH;
    public Renderer letterL;
    public Renderer letterM;
    public Renderer letterO;
    public Renderer letterR;


    public void OnCubePulled()
    {
        
    }

    private void CheckAllPuzzles()
    {
        if (!letters1Colored && mazeController.isBallFinished)
        {
            ColorLetters(letterA, letterH);
            letters1Colored = true;
        }

        if (!letters2Colored)
        {
            ColorLetters(letterL, letterM);
            letters2Colored = true;
        }

        if (!letters3Colored && puzzleGoal.puzzleComplete)
        {
            DrawerAnimator.Play("DrawerOpen");
            ColorLetters(letterO, letterR);
            letters3Colored = true;
        }

        if (mazeController.isBallFinished && picturePuzzleSolved && puzzleGoal.puzzleComplete)
        {
            VaultDoorAnimator.Play("VaultOpen");
        }
    }

    public void ColorLetters(params Renderer[] letters)
    {
        foreach (Renderer r in letters)
        {
            r.material = glowingMaterial;
        }
    }

}
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public Animator DrawerAnimator;
    public Animator VaultDoorAnimator;

    public PicturePuzzle picturePuzzleSolved;

    public MazeFinsihedChecker mazeController;
    public PuzzleGoal puzzleGoal;

    public Material glowingMaterial;

    public MeshRenderer letterA;
    public MeshRenderer letterH;
    public MeshRenderer letterL;
    public MeshRenderer letterM;
    public MeshRenderer letterO;
    public MeshRenderer letterR;


    void Update()
    {
        CheckAllPuzzles();
        if (mazeController.isBallFinished && picturePuzzleSolved && puzzleGoal.puzzleComplete)
        {
            VaultDoorAnimator.Play("VaultOpen");
        }
    }

    private void CheckAllPuzzles()
    {
        if (mazeController.isBallFinished)
        {

            ColorLetters(letterA);
            ColorLetters(letterH);
          
        }

        if (picturePuzzleSolved.isPuzzleFinished)
        {
            ColorLetters(letterL);
            ColorLetters(letterM);
        
        }

        if (puzzleGoal.puzzleComplete)
        {
            DrawerAnimator.Play("DrawerOpen");
            ColorLetters(letterO);
            ColorLetters(letterR);
         
        }
    }

    public void ColorLetters(MeshRenderer letter)
    {
        letter.material = glowingMaterial;
    }

}
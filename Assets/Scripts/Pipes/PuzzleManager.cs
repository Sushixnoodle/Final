using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;
    private PipeRotation[] pipes;

    void Start()
    {
        // Enable the mouse cursor when the game starts
        //  EnableCursor();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void Awake()
    {
        Instance = this;
        pipes = FindObjectsOfType<PipeRotation>();
    }

    public void CheckPuzzleCompletion()
    {
        foreach (PipeRotation pipe in pipes)
        {
            if (!pipe.IsCorrectlyAligned())
            {
                return; // If any pipe is incorrect, do nothing
            }
        }

        Debug.Log("Puzzle Solved! Loading next scene...");
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

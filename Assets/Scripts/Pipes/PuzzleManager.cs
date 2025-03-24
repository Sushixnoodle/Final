using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;
    private PipeRotation[] pipes;

    void Awake()
    {
        Instance = this;
        pipes = FindObjectsOfType<PipeRotation>(true); // Include inactive pipes
    }

    public void CheckPuzzleCompletion()
    {
        Debug.Log("Checking puzzle completion...");

        foreach (PipeRotation pipe in pipes)
        {
            if (!pipe.IsCorrectlyAligned())
            {
                Debug.Log($"Puzzle incomplete due to: {pipe.name}");
                return;
            }
        }

        Debug.Log("ALL PIPES ALIGNED! Loading next scene...");
        LoadNextScene();
    }

    void LoadNextScene()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextScene < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            Debug.LogWarning("No next scene available!");
        }
    }

    [ContextMenu("Force Check All Pipes")]
    public void DebugCheckAllPipes()
    {
        CheckPuzzleCompletion();
    }
}
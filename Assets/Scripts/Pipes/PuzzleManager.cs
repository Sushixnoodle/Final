using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;
    private PipeRotation[] pipes;
    public float successDelay = 0.5f;
    public float angleTolerance = 0.3f; // Exposed tolerance

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            FindAllPipes();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void FindAllPipes()
    {
        pipes = FindObjectsOfType<PipeRotation>(true);
        Debug.Log($"Found {pipes.Length} pipes with tolerance {angleTolerance}°");
    }

    public void CheckPuzzleCompletion()
    {
        StartCoroutine(CheckPuzzleCoroutine());
    }

    private IEnumerator CheckPuzzleCoroutine()
    {
        yield return null; // Wait one frame

        bool allCorrect = true;
        int misalignedCount = 0;

        foreach (PipeRotation pipe in pipes)
        {
            if (!pipe.IsCorrectlyAligned())
            {
                allCorrect = false;
                misalignedCount++;
                // Continue checking all pipes
            }
        }

        if (allCorrect)
        {
            Debug.Log($"PUZZLE SOLVED! (Tolerance: {angleTolerance}°)");
            yield return new WaitForSeconds(successDelay);
            LoadNextScene();
        }
        else
        {
            Debug.Log($"{misalignedCount}/{pipes.Length} pipes misaligned (Tolerance: {angleTolerance}°)");
        }
    }

    private void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No more scenes in build settings!");
        }
    }

    [ContextMenu("Check Alignment with Current Tolerance")]
    public void DebugCheckAlignment()
    {
        FindAllPipes();
        CheckPuzzleCompletion();
    }
}
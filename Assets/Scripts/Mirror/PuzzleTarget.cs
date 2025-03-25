using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleTarget : MonoBehaviour
{
    [Header("Target Settings")]
    public GameObject targetShapePrefab;
    public float completionThreshold = 0.8f;
    public float checkInterval = 0.5f;
    public string nextSceneName;
    public float sceneLoadDelay = 1f;

    [Header("Visual Feedback")]
    public Color inactiveColor = Color.gray;
    public Color activeColor = Color.white;
    public Color completedColor = Color.green;

    private Renderer targetRenderer;
    private LineRenderer targetShape;
    private float lastCheckTime;
    private bool isCompleted = false;

    void Start()
    {
        targetRenderer = GetComponent<Renderer>();
        targetRenderer.material.color = inactiveColor;

        if (targetShapePrefab != null)
        {
            targetShape = Instantiate(targetShapePrefab, transform).GetComponent<LineRenderer>();
            targetShape.startColor = new Color(1, 1, 1, 0.3f);
            targetShape.endColor = new Color(1, 1, 1, 0.3f);
        }
    }

    void Update()
    {
        if (!isCompleted && Time.time - lastCheckTime > checkInterval)
        {
            CheckForCompletion();
            lastCheckTime = Time.time;
        }
    }

    public void ReceiveLightHit(Vector3 hitPoint)
    {
        if (!isCompleted)
        {
            targetRenderer.material.color = activeColor;
        }
    }

    void CheckForCompletion()
    {
        if (isCompleted) return;

        // Get all light points hitting the target
        Collider[] hits = Physics.OverlapBox(transform.position, transform.localScale / 2);
        int activeHits = 0;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("LightPoint"))
            {
                activeHits++;
            }
        }

        // Simple completion check - you can expand this with pattern matching
        if (activeHits > 0)
        {
            float completionRatio = (float)activeHits / targetShape.positionCount;
            if (completionRatio >= completionThreshold)
            {
                CompletePuzzle();
            }
        }
        else
        {
            targetRenderer.material.color = inactiveColor;
        }
    }

    void CompletePuzzle()
    {
        isCompleted = true;
        targetRenderer.material.color = completedColor;
        Debug.Log("Puzzle Completed!");

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Invoke("LoadNextScene", sceneLoadDelay);
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
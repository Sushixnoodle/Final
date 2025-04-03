using UnityEngine;
using UnityEngine.SceneManagement;

public class TargetScript : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Number of reflections required before activating")]
    public int requiredReflections = 3;
    [Tooltip("Name of the scene to load when activated")]
    public string nextSceneName = "NextLevel";
    [Tooltip("Color change when hit (for visual feedback)")]
    public Color activatedColor = Color.green;

    [Header("Debug")]
    public bool showDebugMessages = true;

    private Renderer targetRenderer;
    private Color originalColor;
    private int currentReflectionCount = 0;

    private void Start()
    {
        targetRenderer = GetComponent<Renderer>();
        originalColor = targetRenderer.material.color;

        if (showDebugMessages)
        {
            Debug.Log($"Target initialized. Requires {requiredReflections} reflections.");
        }
    }

    public void RegisterLaserHit(int reflectionCount)
    {
        currentReflectionCount = reflectionCount;

        if (showDebugMessages)
        {
            Debug.Log($"Target hit! Reflection count: {reflectionCount}/{requiredReflections}");
        }

        if (reflectionCount >= requiredReflections)
        {
            ActivateTarget();
        }
        else
        {
            // Visual feedback for partial progress
            targetRenderer.material.color = Color.Lerp(originalColor, activatedColor,
                (float)reflectionCount / requiredReflections);

            // Reset after short delay
            Invoke("ResetColor", 0.5f);
        }
    }

    private void ActivateTarget()
    {
        targetRenderer.material.color = activatedColor;

        if (showDebugMessages)
        {
            Debug.Log("Target activated! Loading scene: " + nextSceneName);
        }

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }

    private void ResetColor()
    {
        targetRenderer.material.color = originalColor;
    }

    // Visual indicator in editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
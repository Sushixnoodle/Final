using UnityEngine;
using UnityEngine.SceneManagement;

public class LaserTarget : MonoBehaviour
{
    [Header("Settings")]
    public string nextSceneName; // Name of the scene to load
    public Color hitColor = Color.green; // Visual feedback when hit
    public float transitionDelay = 0.5f; // Delay before scene transition

    private Renderer objectRenderer;
    private Color originalColor;
    private bool isHit = false;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
    }

    // Called when hit by the laser (compatible with both systems)
    public void OnLaserHit()
    {
        if (isHit) return; // Prevent multiple triggers

        isHit = true;

        // Visual feedback
        if (objectRenderer != null)
        {
            objectRenderer.material.color = hitColor;
        }

        // Load next scene after delay
        Invoke("LoadNextScene", transitionDelay);
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Next scene name not specified on LaserTarget");
        }
    }
}
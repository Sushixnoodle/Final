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

    private void Update()
    {
        //OnLaserHit(hit);
    }

    // Called when hit by the laser (compatible with both systems)
    public void OnLaserHit(RaycastHit hit)
    {
        if(hit.collider.CompareTag("Mirror"))
        {
            isHit = true;
            objectRenderer.material.color = hitColor;
            LoadNextScene();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            Debug.Log("IM BEING TOUCHED BY LASER");
            isHit = true;
            objectRenderer.material.color = hitColor;
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        Debug.Log("PREPARING TELELPORT!!!!!");
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
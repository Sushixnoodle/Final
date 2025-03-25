using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PipeLoad : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The object that triggers scene change when enabled")]
    public GameObject triggerObject;
    [Tooltip("Delay in seconds before loading next scene")]
    public float delayBeforeLoad = 3f;
    [Tooltip("Name or index of the next scene to load")]
    public string nextSceneName;
    public int nextSceneIndex = -1;

    private bool sceneLoadInitiated = false;

    private void Update()
    {
        // Check if trigger object exists and is active
        if (!sceneLoadInitiated && triggerObject != null && triggerObject.activeInHierarchy)
        {
            sceneLoadInitiated = true;
            Invoke("LoadNextScene", delayBeforeLoad);
        }
    }

    private void LoadNextScene()
    {
        // Load by name if specified, otherwise by index
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else if (nextSceneIndex >= 0)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No valid scene specified for loading!");
        }
    }
}
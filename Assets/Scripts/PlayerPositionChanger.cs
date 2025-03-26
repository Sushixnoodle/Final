using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPositionChanger : MonoBehaviour
{
    // The two scenes between which we'll save position
    public string firstSceneName;
    public string secondSceneName;

    private static Vector3 savedPosition;
    private static string lastScene;

    void Start()
    {
        // If coming back to the second scene from the first scene
        if (SceneManager.GetActiveScene().name == secondSceneName &&
            lastScene == firstSceneName)
        {
            transform.position = savedPosition;
        }
    }

    void OnDisable()
    {
        // If moving from first scene to second scene
        if (SceneManager.GetActiveScene().name == firstSceneName &&
            SceneManager.GetSceneByName(secondSceneName).IsValid())
        {
            savedPosition = transform.position;
            lastScene = firstSceneName;
        }
    }
}
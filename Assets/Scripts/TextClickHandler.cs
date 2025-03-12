using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TextClickHandler : MonoBehaviour
{
    // Assign these in the Inspector
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI text3;

    private int clickCount = 0;

    void Update()
    {
        // Check for left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            clickCount++;

            // Handle text transitions based on click count
            switch (clickCount)
            {
                case 1:
                    // Disable Text1 and enable Text2
                    text1.gameObject.SetActive(false);
                    text2.gameObject.SetActive(true);
                    break;

                case 2:
                    // Disable Text2 and enable Text3
                    text2.gameObject.SetActive(false);
                    text3.gameObject.SetActive(true);
                    break;

                case 3:
                    // Load the next scene
                    LoadNextScene();
                    break;
            }
        }
    }

    void LoadNextScene()
    {
        // Load the next scene in the build settings
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No next scene available. Add a scene to the build settings.");
        }
    }
}
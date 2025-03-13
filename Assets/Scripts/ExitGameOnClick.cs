using UnityEngine;

public class ExitGameOnClick : MonoBehaviour
{
    // This method is called when the button is clicked
    public void OnButtonClick()
    {
        // Quit the application
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Exit play mode in the editor
#else
            Application.Quit(); // Quit the application in a build
#endif
    }
}
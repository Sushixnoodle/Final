using UnityEngine;

public class EnableMouseCursor : MonoBehaviour
{
    void Start()
    {
        // Enable the mouse cursor when the game starts
        EnableCursor();
    }

    void EnableCursor()
    {
        // Make the mouse cursor visible
        Cursor.visible = true;

        // Unlock the cursor so it can move freely
        Cursor.lockState = CursorLockMode.None;
    }
}
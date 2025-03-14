using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMouseCursor : MonoBehaviour
{
    void Start()
    {
        // Disable the mouse cursor completely when the game starts
        DisableCursor();
    }

    void DisableCursor()
    {
        // Hide the mouse cursor
        Cursor.visible = false;

        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }
}
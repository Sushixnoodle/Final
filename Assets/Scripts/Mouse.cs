using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    private bool isMouseEnabled = true; // Tracks the current state of the mouse

    void Update()
    {
        // Check if the "M" key is pressed
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // Toggle the mouse state
            isMouseEnabled = !isMouseEnabled;

            // Update the mouse visibility and lock state
            UpdateMouseState();
        }
    }

    void UpdateMouseState()
    {
        if (isMouseEnabled)
        {
            // Enable the mouse cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        }
        else
        {
            // Disable the mouse cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        }
    }
}
using UnityEngine;

public class MouseVisibilityController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag the Dialogue Panel here")]
    public GameObject dialoguePanel;

    [Header("Mouse Settings")]
    [Tooltip("Should the mouse be locked when panel is closed?")]
    public bool lockMouseWhenPanelClosed = true;
    [Tooltip("Should the mouse be visible only when panel is open?")]
    public bool hideMouseWhenPanelClosed = true;

    private void Update()
    {
        if (dialoguePanel == null)
        {
            Debug.LogWarning("Dialogue Panel reference is missing in MouseVisibilityController!");
            return;
        }

        // If panel is active, show & unlock mouse
        if (dialoguePanel.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        // If panel is inactive, hide & lock mouse
        else
        {
            if (lockMouseWhenPanelClosed)
                Cursor.lockState = CursorLockMode.Locked;

            if (hideMouseWhenPanelClosed)
                Cursor.visible = false;
        }
    }
}
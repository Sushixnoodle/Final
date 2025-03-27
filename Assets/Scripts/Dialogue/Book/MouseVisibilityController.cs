using UnityEngine;

public class MouseVisibilityController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject bookUIPanel;        // The book UI panel
    public GameObject otherPanel;         // Panel to disable when book is open

    [Header("Player Control")]
    public Movement playerMovement;
    public Rigidbody playerRigidbody;    // Optional for physics-based characters

    [Header("Mouse Settings")]
    public bool lockMouseWhenClosed = true;
    public bool hideMouseWhenClosed = true;

    // Store original rigidbody state
    private bool wasKinematic;
    private Vector3 originalVelocity;
    private Vector3 originalAngularVelocity;

    private void Update()
    {
        if (bookUIPanel != null)
        {
            bool bookActive = bookUIPanel.activeSelf;

            // Update other panel state
            if (otherPanel != null)
            {
                if (bookActive && otherPanel.activeSelf)
                    otherPanel.SetActive(false);
                else if (!bookActive && !otherPanel.activeSelf)
                    otherPanel.SetActive(true);
            }

            // Update player and mouse states
            UpdatePlayerState(bookActive);
            UpdateMouseState(bookActive);
        }
    }

    private void UpdatePlayerState(bool freeze)
    {
        if (playerMovement != null)
            playerMovement.enabled = !freeze;

        if (playerRigidbody != null)
        {
            if (freeze)
            {
                // Save state before freezing
                wasKinematic = playerRigidbody.isKinematic;
                originalVelocity = playerRigidbody.velocity;
                originalAngularVelocity = playerRigidbody.angularVelocity;

                // Freeze by making kinematic
                playerRigidbody.isKinematic = true;
            }
            else
            {
                // Unfreeze
                playerRigidbody.isKinematic = wasKinematic;

                // Only restore velocity if not kinematic
                if (!wasKinematic)
                {
                    playerRigidbody.velocity = originalVelocity;
                    playerRigidbody.angularVelocity = originalAngularVelocity;
                }
            }
        }
    }

    private void UpdateMouseState(bool bookActive)
    {
        if (bookActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = lockMouseWhenClosed ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !hideMouseWhenClosed;
        }
    }

    // Handle scene changes
    private void OnEnable()
    {
        // Reset states when script is enabled (like after scene load)
        if (playerRigidbody != null && !playerRigidbody.isKinematic)
        {
            originalVelocity = playerRigidbody.velocity;
            originalAngularVelocity = playerRigidbody.angularVelocity;
        }
    }
}
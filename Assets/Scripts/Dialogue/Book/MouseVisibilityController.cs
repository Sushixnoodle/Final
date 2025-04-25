using UnityEngine;

public class MouseVisibilityController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject bookUIPanel;        // The book UI panel
    public GameObject otherPanel;         // Panel to disable when book is open
    public GameObject infoText;           // Text or tooltip to hide while book is open

    [Header("Player Control")]
    public Movement playerMovement;
    public Rigidbody playerRigidbody;     // Optional for physics-based characters

    [Header("Mouse Settings")]
    public bool lockMouseWhenClosed = true;
    public bool hideMouseWhenClosed = true;

    // Internal state tracking
    private bool wasKinematic;
    private Vector3 originalVelocity;
    private Vector3 originalAngularVelocity;
    private bool shouldShowInfoText;      // Whether infoText should be visible normally

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

            // Update infoText state
            if (infoText != null)
            {
                if (bookActive)
                {
                    if (infoText.activeSelf)
                        infoText.SetActive(false);
                }
                else
                {
                    if (shouldShowInfoText && !infoText.activeSelf)
                        infoText.SetActive(true);
                }
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
                wasKinematic = playerRigidbody.isKinematic;
                originalVelocity = playerRigidbody.velocity;
                originalAngularVelocity = playerRigidbody.angularVelocity;
                playerRigidbody.isKinematic = true;
            }
            else
            {
                playerRigidbody.isKinematic = wasKinematic;
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
        if (playerRigidbody != null && !playerRigidbody.isKinematic)
        {
            originalVelocity = playerRigidbody.velocity;
            originalAngularVelocity = playerRigidbody.angularVelocity;
        }
    }

    // Trigger logic to manage infoText state
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldShowInfoText = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldShowInfoText = false;
        }
    }
}

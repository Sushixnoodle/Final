using UnityEngine;

public class MouseVisibilityController : MonoBehaviour
{
    [Header("References")]
    public GameObject dialoguePanel;
    public Movement playerMovement;
    public Rigidbody playerRigidbody; // Optional - only if using Rigidbody

    [Header("Settings")]
    public bool lockMouseWhenClosed = true;
    public bool hideMouseWhenClosed = true;

    private Vector3 savedVelocity;
    private Vector3 savedAngularVelocity;
    private bool wasKinematic;

    private void Start()
    {
        if (playerMovement == null)
            playerMovement = FindObjectOfType<Movement>();

        if (playerRigidbody == null)
            playerRigidbody = playerMovement?.GetComponent<Rigidbody>();

        UpdateState(dialoguePanel != null && dialoguePanel.activeSelf);
    }

    private void Update()
    {
        if (dialoguePanel != null)
            UpdateState(dialoguePanel.activeSelf);
    }

    private void UpdateState(bool panelActive)
    {
        if (panelActive)
        {
            // Freeze everything
            if (playerMovement != null)
                playerMovement.enabled = false;

            if (playerRigidbody != null)
            {
                savedVelocity = playerRigidbody.velocity;
                savedAngularVelocity = playerRigidbody.angularVelocity;
                wasKinematic = playerRigidbody.isKinematic;
                playerRigidbody.isKinematic = true;
            }

            // Mouse control
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Unfreeze everything
            if (playerMovement != null)
                playerMovement.enabled = true;

            if (playerRigidbody != null)
            {
                playerRigidbody.isKinematic = wasKinematic;
                if (!wasKinematic)
                {
                    playerRigidbody.velocity = savedVelocity;
                    playerRigidbody.angularVelocity = savedAngularVelocity;
                }
            }

            // Mouse control
            Cursor.lockState = lockMouseWhenClosed ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !hideMouseWhenClosed;
        }
    }
}
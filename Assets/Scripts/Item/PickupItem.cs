using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("Interaction Settings")]
    [Tooltip("Maximum interaction distance")]
    public float maxInteractionDistance = 3f;
    [Tooltip("Key to press for interaction (default: left mouse)")]
    public KeyCode interactionKey = KeyCode.Mouse0;

    [Header("UI Settings")]
    [Tooltip("The UI Image to enable when this item is picked up")]
    public GameObject imageToEnable;
    [Tooltip("UI prompt to show when item is in range")]
    public GameObject interactionPrompt;

    [Header("Object Control")]
    [Tooltip("Objects to enable when this item is picked up")]
    public GameObject[] objectsToEnable;
    [Tooltip("Objects to disable when this item is picked up")]
    public GameObject[] objectsToDisable;

    [Header("Audio")]
    [Tooltip("Sound to play when picked up")]
    public AudioClip pickupSound;

    private AudioSource audioSource;
    private Camera playerCamera;
    private bool isInRange = false;

    private void Start()
    {
        // Set up audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && pickupSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Find player camera
        playerCamera = Camera.main;

        // Hide interaction prompt at start
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    private void Update()
    {
        CheckPlayerLooking();

        // Check for interaction key press when in range
        if (isInRange && Input.GetKeyDown(interactionKey))
        {
            PickUp();
        }
    }

    private void CheckPlayerLooking()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxInteractionDistance))
        {
            bool nowInRange = hit.collider.gameObject == gameObject;

            // Only update if state changed
            if (nowInRange != isInRange)
            {
                isInRange = nowInRange;
                UpdateInteractionPrompt();
            }
        }
        else if (isInRange)
        {
            isInRange = false;
            UpdateInteractionPrompt();
        }
    }

    private void UpdateInteractionPrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(isInRange);
        }
    }

    public void PickUp()
    {
        // Play sound
        if (pickupSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(pickupSound);
        }

        // Enable UI image
        if (imageToEnable != null)
        {
            imageToEnable.SetActive(true);
        }

        // Handle objects to enable
        foreach (GameObject obj in objectsToEnable)
        {
            if (obj != null) obj.SetActive(true);
        }

        // Handle objects to disable
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null) obj.SetActive(false);
        }

        // Hide interaction prompt
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }

        // Destroy the pickup item
        Destroy(gameObject);
    }
}
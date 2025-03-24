using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The UI Image to enable when this item is picked up")]
    public GameObject imageToEnable;

    [Tooltip("The GameObject to enable when this item is picked up")]
    public GameObject objectToEnable;

    [Tooltip("Sound to play when picked up (optional)")]
    public AudioClip pickupSound;

    private AudioSource audioSource;

    private void Start()
    {
        // Try to get an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && pickupSound != null)
        {
            // If there's no AudioSource but we have a sound, add one
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is the player (you might want to change this tag check)
        if (other.CompareTag("Player"))
        {
            PickUp();
        }
    }

    public void PickUp()
    {
        // Play pickup sound if available
        if (pickupSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(pickupSound);
        }

        // Enable the image if assigned
        if (imageToEnable != null)
        {
            imageToEnable.SetActive(true);
        }

        // Enable the other object if assigned
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }

        // Destroy the pickup item
        Destroy(gameObject);
    }
}
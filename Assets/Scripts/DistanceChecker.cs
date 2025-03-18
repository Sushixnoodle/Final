using UnityEngine;
using TMPro; // Add this namespace for TextMeshPro

public class DistanceChecker : MonoBehaviour
{
    public GameObject partNameText; // The UI or 3D text to show/hide
    public Transform player; // Reference to the player or object to measure distance to
    public float activationDistance = 5f; // Distance at which the text appears

    private void Start()
    {
        // Ensure the text is hidden at the start
        if (partNameText != null)
        {
            partNameText.SetActive(false);
        }
    }

    private void Update()
    {
        // Check if the player reference is set
        if (player == null)
        {
            Debug.LogWarning("Player reference is not set.");
            return;
        }

        // Calculate the distance between this object and the player
        float distance = Vector3.Distance(transform.position, player.position);

        // Show or hide the text based on the distance
        if (distance <= activationDistance)
        {
            partNameText.SetActive(true);
        }
        else
        {
            partNameText.SetActive(false);
        }
    }
}
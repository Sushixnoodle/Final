using UnityEngine;

public class RotatableObject : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("How close the player needs to be to interact")]
    public float activationDistance = 5f;
    [Tooltip("Rotation speed in degrees per second")]
    public float rotationSpeed = 90f;
    [Tooltip("Should rotation be smoothed?")]
    public bool smoothRotation = true;
    [Tooltip("Smoothing speed (if enabled)")]
    public float smoothTime = 0.2f;

    [Header("References")]
    [Tooltip("Reference to the player transform (drag player here)")]
    public Transform player;
    [Tooltip("Optional highlight effect when in range")]
    public GameObject highlightEffect;

    private float currentVelocity; // For smooth damping
    private float targetRotation; // For smooth rotation
    private bool playerInRange;

    private void Update()
    {
        // Check player distance
        float distance = Vector3.Distance(transform.position, player.position);
        playerInRange = distance <= activationDistance;

        // Toggle highlight effect if assigned
        if (highlightEffect != null)
            highlightEffect.SetActive(playerInRange);

        // Only allow rotation when player is in range
        if (playerInRange)
        {
            HandleRotationInput();
        }

        ApplyRotation();
    }

    private void HandleRotationInput()
    {
        // Rotate left while holding left mouse button
        if (Input.GetMouseButton(0))
        {
            targetRotation -= rotationSpeed * Time.deltaTime;
        }
        // Rotate right while holding right mouse button
        else if (Input.GetMouseButton(1))
        {
            targetRotation += rotationSpeed * Time.deltaTime;
        }
    }

    private void ApplyRotation()
    {
        if (smoothRotation)
        {
            // Smooth damp the rotation
            float currentAngle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetRotation,
                ref currentVelocity,
                smoothTime
            );
            transform.rotation = Quaternion.Euler(0, currentAngle, 0);
        }
        else
        {
            // Instant rotation
            transform.rotation = Quaternion.Euler(0, targetRotation, 0);
        }
    }

    // Visualize activation distance in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activationDistance);
    }
}
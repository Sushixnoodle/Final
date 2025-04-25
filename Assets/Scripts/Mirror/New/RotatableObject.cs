using UnityEngine;

public class RotatableObject : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float activationDistance = 5f;
    public float rotationSpeed = 90f;
    public bool smoothRotation = true;
    public float smoothTime = 0.2f;

    [Header("References")]
    public Transform player;
    public GameObject highlightEffect;
    public AudioSource rotationAudioSource; // Drag your audio source here

    private float currentVelocity;
    private float targetRotation;
    private bool playerInRange;
    private bool isRotating;

    private void HandleAudio()
    {
        if (rotationAudioSource == null) return;

        if (isRotating)
        {
            if (!rotationAudioSource.isPlaying)
            {
                rotationAudioSource.volume = 1f;
                rotationAudioSource.Play();
            }
        }
        else
        {
            if (rotationAudioSource.isPlaying)
            {
                rotationAudioSource.Stop();
            }
        }
    }
    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        playerInRange = distance <= activationDistance;

        if (highlightEffect != null)
            highlightEffect.SetActive(playerInRange);

        bool wasRotating = isRotating;
        isRotating = false;

        if (playerInRange)
        {
            HandleRotationInput();
        }

        ApplyRotation();

        // Check if rotation started or stopped
        if (isRotating && !wasRotating)
        {
            if (rotationAudioSource != null && !rotationAudioSource.isPlaying)
                rotationAudioSource.Play();
        }
        else if (!isRotating && wasRotating)
        {
            if (rotationAudioSource != null && rotationAudioSource.isPlaying)
                rotationAudioSource.Stop();
        }
    }

    private void HandleRotationInput()
    {
        if (Input.GetMouseButton(0))
        {
            targetRotation -= rotationSpeed * Time.deltaTime;
            isRotating = true;
        }
        else if (Input.GetMouseButton(1))
        {
            targetRotation += rotationSpeed * Time.deltaTime;
            isRotating = true;
        }
    }

    private void ApplyRotation()
    {
        if (smoothRotation)
        {
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
            transform.rotation = Quaternion.Euler(0, targetRotation, 0);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activationDistance);
    }
}

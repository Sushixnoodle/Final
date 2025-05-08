using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RaySystem : MonoBehaviour
{
    [Header("Ray Settings")]
    public int maxBounces = 10;
    public float maxDistance = 100f;
    public float rayOffset = 0.01f;

    [Header("Layer Masks")]
    public LayerMask reflectionLayers; // Layers that reflect the ray
    public LayerMask blockingLayers;   // Layers that stop the ray

    [Header("References")]
    public LineRenderer line;
    public AudioClip targetHitSound; // Assign in inspector
    public Transform lastMirror; // Drag the last mirror in inspector to freeze its rotation

    [Header("Target Settings")]
    public Color hitColor = Color.green;
    public float activationDelay = 3f; // 3 second delay after hit

    [Header("Objects to Enable")]
    public GameObject[] objectsToEnable; // Assign objects in inspector
    [Header("Objects to Disable")]
    public GameObject[] objectsToDisable; // Assign objects in inspector
    public GameObject textPrompt; // Assign text prompt in inspector

    private AudioSource audioSource;
    private bool targetHit = false;
    private Quaternion lastMirrorRotationWhenHit;
    private bool mirrorRotationFrozen = false;

    void Start()
    {
        if (line == null) line = GetComponent<LineRenderer>();
        line.useWorldSpace = true;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Ensure objects start in their correct states
        if (objectsToEnable != null)
        {
            foreach (GameObject obj in objectsToEnable)
            {
                if (obj != null) obj.SetActive(false);
            }
        }

        if (objectsToDisable != null)
        {
            foreach (GameObject obj in objectsToDisable)
            {
                if (obj != null) obj.SetActive(true);
            }
        }

        if (textPrompt != null) textPrompt.SetActive(false);
    }

    void Update()
    {
        UpdateLaser();

        // Freeze last mirror's rotation if target was hit
        if (mirrorRotationFrozen && lastMirror != null)
        {
            lastMirror.rotation = lastMirrorRotationWhenHit;
        }
    }

    void UpdateLaser()
    {
        // Reset line
        line.positionCount = 1;
        line.SetPosition(0, transform.position);

        Vector3 currentPos = transform.position;
        Vector3 currentDir = transform.forward;
        int bounceCount = 0;

        while (bounceCount < maxBounces)
        {
            RaycastHit hit;
            bool didHit = Physics.Raycast(currentPos, currentDir, out hit, maxDistance, reflectionLayers | blockingLayers);

            if (!didHit)
            {
                AddLinePoint(currentPos + currentDir * maxDistance);
                break;
            }

            AddLinePoint(hit.point);

            // Check if we hit a blocking layer
            if (((1 << hit.collider.gameObject.layer) & blockingLayers) != 0)
            {
                // Check for target hit on blocking layer
                if (hit.collider.CompareTag("Target") && !targetHit)
                {
                    OnTargetHit(hit);
                }
                break;
            }

            // Handle reflection
            if (((1 << hit.collider.gameObject.layer) & reflectionLayers) != 0)
            {
                // Check for target hit on reflection layer
                if (hit.collider.CompareTag("Target") && !targetHit)
                {
                    OnTargetHit(hit);
                }

                currentPos = hit.point + (hit.normal * rayOffset);
                currentDir = Vector3.Reflect(currentDir, hit.normal);
                bounceCount++;
            }
        }
    }

    void OnTargetHit(RaycastHit hit)
    {
        targetHit = true;

        // Store the current rotation of the last mirror
        if (lastMirror != null)
        {
            lastMirrorRotationWhenHit = lastMirror.rotation;
            mirrorRotationFrozen = true;
        }

        // Visual feedback
        if (hit.collider.TryGetComponent<Renderer>(out Renderer targetRenderer))
        {
            targetRenderer.material.color = hitColor;
        }

        // Play sound
        if (targetHitSound != null)
        {
            audioSource.PlayOneShot(targetHitSound);
        }

        // Start activation coroutine
        StartCoroutine(DelayedActivation());
    }

    IEnumerator DelayedActivation()
    {
        yield return new WaitForSeconds(activationDelay);

        // Enable all assigned objects
        if (objectsToEnable != null)
        {
            foreach (GameObject obj in objectsToEnable)
            {
                if (obj != null) obj.SetActive(true);
            }
        }

        // Disable all assigned objects
        if (objectsToDisable != null)
        {
            foreach (GameObject obj in objectsToDisable)
            {
                if (obj != null) obj.SetActive(false);
            }
        }

        // Enable text prompt
        if (textPrompt != null) textPrompt.SetActive(true);
    }

    void AddLinePoint(Vector3 point)
    {
        line.positionCount++;
        line.SetPosition(line.positionCount - 1, point);
    }
}
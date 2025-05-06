using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public AudioClip hitSound; // Assign in inspector
    public Transform lastMirror; // Assign the last mirror in inspector

    [Header("Target Settings")]
    public string nextSceneName;
    public Color hitColor = Color.green;
    public float sceneTransitionDelay = 3f; // 3 second delay

    private AudioSource audioSource;
    private bool targetHit = false;
    private Quaternion initialMirrorRotation;

    void Start()
    {
        if (line == null) line = GetComponent<LineRenderer>();
        line.useWorldSpace = true;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (lastMirror != null)
        {
            initialMirrorRotation = lastMirror.rotation;
        }
    }

    void Update()
    {
        UpdateLaser();

        // Freeze last mirror's rotation if target was hit
        if (targetHit && lastMirror != null)
        {
            lastMirror.rotation = initialMirrorRotation;
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

            // Check for target hit
            if (hit.collider.CompareTag("Target") && !targetHit)
            {
                StartCoroutine(TargetHitSequence(hit));
                targetHit = true;
            }

            // Check if we hit a blocking layer
            if (((1 << hit.collider.gameObject.layer) & blockingLayers) != 0)
            {
                break;
            }

            // Handle reflection
            if (((1 << hit.collider.gameObject.layer) & reflectionLayers) != 0)
            {
                currentPos = hit.point + (hit.normal * rayOffset);
                currentDir = Vector3.Reflect(currentDir, hit.normal);
                bounceCount++;
            }
        }
    }

    IEnumerator TargetHitSequence(RaycastHit hit)
    {
        // Visual feedback
        if (hit.collider.TryGetComponent<Renderer>(out Renderer targetRenderer))
        {
            targetRenderer.material.color = hitColor;
        }

        // Play sound
        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
        else
        {
            Debug.LogWarning("No hit sound assigned!");
        }

        // Freeze last mirror (handled in Update)

        // Wait before scene transition
        yield return new WaitForSeconds(sceneTransitionDelay);

        // Load next scene
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    void AddLinePoint(Vector3 point)
    {
        line.positionCount++;
        line.SetPosition(line.positionCount - 1, point);
    }
}
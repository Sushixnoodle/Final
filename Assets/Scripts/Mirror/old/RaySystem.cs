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
    public LayerMask reflectionLayers; // Layers that reflect the ray (e.g., mirrors)
    public LayerMask blockingLayers;   // Layers that stop the ray (e.g., walls)

    [Header("References")]
    public LineRenderer line;

    [Header("Target Detection")]
    public string nextSceneName;
    public Color hitColor = Color.green;
    public float transitionDelay = 0.5f;

    private bool isHit = false;

    void Start()
    {
        if (line == null) line = GetComponent<LineRenderer>();
        line.useWorldSpace = true;
    }

    void Update()
    {
        UpdateLaser();
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
                // No hit - draw ray to max distance
                AddLinePoint(currentPos + currentDir * maxDistance);
                break;
            }

            // Add hit point to line
            AddLinePoint(hit.point);

            // Check if we hit a blocking layer
            if (((1 << hit.collider.gameObject.layer) & blockingLayers) != 0)
            {
                // Hit a blocking object - stop here
                if (hit.collider.CompareTag("Target"))
                {
                    StartCoroutine(OnLaserHit(hit));
                }
                break;
            }

            // Handle reflection
            if (((1 << hit.collider.gameObject.layer) & reflectionLayers) != 0)
            {
                // Reflect off mirror-like surfaces
                currentPos = hit.point + (hit.normal * rayOffset);
                currentDir = Vector3.Reflect(currentDir, hit.normal);
                bounceCount++;
            }

            // Check for target even on reflectable surfaces
            if (hit.collider.CompareTag("Target"))
            {
                StartCoroutine(OnLaserHit(hit));
            }
        }
    }

    void AddLinePoint(Vector3 point)
    {
        line.positionCount++;
        line.SetPosition(line.positionCount - 1, point);
    }

    IEnumerator OnLaserHit(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent<Renderer>(out Renderer targetRenderer))
        {
            targetRenderer.material.color = hitColor;
        }
        isHit = true;
        yield return new WaitForSeconds(transitionDelay);
        LoadNextScene();
    }

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
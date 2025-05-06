using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaySystem : MonoBehaviour
{
    [Header("Settings")]
    public int maxBounces = 10;
    public float maxDistance = 100f;
    public LayerMask reflectionLayers = ~0;
    public float rayOffset = 0.1f;

    [Header("References")]
    public LineRenderer line;

    [Header("Detection System")]
    public string nextSceneName;
    public Color hitColor = Color.green;
    public float transitionDelay = 0.5f;

    private bool isHit = false;

    void Start()
    {
        if (line == null) line = GetComponent<LineRenderer>();
        line.useWorldSpace = true;
        // Initialize with minimum positions (2: start and end)
        line.positionCount = 2;
    }

    void Update()
    {
        UpdateLaser();
    }

    void UpdateLaser()
    {
        // Start with just start and end points
        line.positionCount = 2;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + transform.forward * maxDistance);

        Vector3 currentPos = transform.position;
        Vector3 currentDir = transform.forward;
        int bounceCount = 0;

        // Temporary list to store all points
        List<Vector3> points = new List<Vector3>();
        points.Add(currentPos);

        for (int i = 0; i < maxBounces; i++)
        {
            if (Physics.Raycast(currentPos, currentDir, out RaycastHit hit, maxDistance, reflectionLayers))
            {
                points.Add(hit.point);
                Debug.DrawLine(currentPos, hit.point, Color.green, 0.1f);

                if (hit.collider.CompareTag("Target"))
                {
                    StartCoroutine(OnLaserHit(hit));
                }

                currentPos = hit.point + (hit.normal * rayOffset);
                currentDir = Vector3.Reflect(currentDir, hit.normal);
                bounceCount++;
            }
            else
            {
                points.Add(currentPos + currentDir * maxDistance);
                Debug.DrawRay(currentPos, currentDir * maxDistance, Color.red, 0.1f);
                break;
            }
        }

        // Apply all points to the LineRenderer
        line.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            line.SetPosition(i, points[i]);
        }
    }

    IEnumerator OnLaserHit(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent<Renderer>(out Renderer targetRenderer))
        {
            targetRenderer.material.color = hitColor;
        }
        else
        {
            Debug.LogWarning("Target does not have renderer component");
        }

        isHit = true;
        yield return new WaitForSeconds(transitionDelay);
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Next scene name not specified on LaserTarget");
        }
    }
}
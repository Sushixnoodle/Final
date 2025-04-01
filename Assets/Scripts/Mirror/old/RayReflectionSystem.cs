using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RayReflectionSystem : MonoBehaviour
{
    [Header("Ray Settings")]
    [SerializeField] private int maxReflections = 5; // Max number of bounces
    [SerializeField] private float maxRayDistance = 100f; // Max distance per ray
    [SerializeField] private LayerMask reflectionLayers; // Layers the ray can hit

    [Header("Line Renderer")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Color rayColor = Color.red;

    private void Start()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.startColor = rayColor;
        lineRenderer.endColor = rayColor;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    private void Update()
    {
        CastReflectionRay(transform.position, transform.forward);
    }

    private void CastReflectionRay(Vector3 origin, Vector3 direction)
    {
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, origin);

        Vector3 currentPos = origin;
        Vector3 currentDir = direction;

        for (int i = 0; i < maxReflections; i++)
        {
            Ray ray = new Ray(currentPos, currentDir);
            if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, reflectionLayers))
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(i + 1, hit.point);

                // Reflect the ray
                currentDir = Vector3.Reflect(currentDir, hit.normal);
                currentPos = hit.point;

                // Debug visualization
                Debug.DrawLine(currentPos, hit.point, Color.red, 0.1f);
                Debug.DrawRay(hit.point, hit.normal, Color.green, 0.1f);
            }
            else
            {
                // Ray exits into space
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(i + 1, currentPos + currentDir * maxRayDistance);
                Debug.DrawRay(currentPos, currentDir * maxRayDistance, Color.blue, 0.1f);
                break;
            }
        }
    }
}
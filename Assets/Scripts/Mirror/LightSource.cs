using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LightSource : MonoBehaviour
{
    [Header("Light Settings")]
    public float maxDistance = 100f;
    public Color lightColor = Color.yellow;
    public LayerMask reflectionLayer;

    [Header("Visual Settings")]
    public float lineWidth = 0.1f;

    private LineRenderer lineRenderer;
    private bool isActive = true;

    void Start()
    {
        InitializeLineRenderer();
    }

    void Update()
    {
        if (isActive)
        {
            lineRenderer.positionCount = 0;
            CastLight(transform.position, transform.forward, maxDistance);
        }
    }

    void InitializeLineRenderer()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = lightColor;
        lineRenderer.endColor = lightColor;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 0;
    }

    void CastLight(Vector3 position, Vector3 direction, float remainingDistance)
    {
        if (lineRenderer.positionCount >= 50) return; // Safety check

        Ray ray = new Ray(position, direction);
        RaycastHit hit;

        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);

        if (Physics.Raycast(ray, out hit, remainingDistance, reflectionLayer))
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

            if (hit.collider.CompareTag("Mirror"))
            {
                Vector3 reflectDir = Vector3.Reflect(direction, hit.normal);
                float distanceTraveled = Vector3.Distance(position, hit.point);
                CastLight(hit.point, reflectDir, remainingDistance - distanceTraveled);
            }
            else if (hit.collider.CompareTag("PuzzleTarget"))
            {
                PuzzleTarget target = hit.collider.GetComponent<PuzzleTarget>();
                if (target != null)
                {
                    target.ReceiveLightHit(hit.point);
                }
            }
        }
        else
        {
            Vector3 endPoint = position + direction * remainingDistance;
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, endPoint);
        }
    }

    public void SetActive(bool active)
    {
        isActive = active;
        if (!active) lineRenderer.positionCount = 0;
    }
}
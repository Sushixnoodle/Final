using UnityEngine;

public class RaySystem : MonoBehaviour
{
    [Header("Settings")]
    public int maxBounces = 10;
    public float maxDistance = 100f;
    public LayerMask reflectionLayers = ~0; // Everything (temporarily)
    public float rayOffset = 0.01f;

    [Header("References")]
    public LineRenderer line;

    void Start()
    {
        if (line == null) line = GetComponent<LineRenderer>();
        line.useWorldSpace = true; // Critical!
        line.positionCount = maxBounces + 1;
    }

    void Update()
    {
        UpdateLaser();
    }

    void UpdateLaser()
    {
        // Reset to emitter position first
        for (int i = 0; i < line.positionCount; i++)
        {
            line.SetPosition(i, transform.position);
        }

        Vector3 currentPos = transform.position;
        Vector3 currentDir = transform.forward;
        line.SetPosition(0, currentPos);

        for (int i = 0; i < maxBounces; i++)
        {
            if (Physics.Raycast(currentPos, currentDir, out RaycastHit hit, maxDistance, reflectionLayers))
            {
                line.SetPosition(i + 1, hit.point);
                Debug.DrawLine(currentPos, hit.point, Color.green, 0.1f);

                // Offset to avoid self-collision
                currentPos = hit.point + (hit.normal * rayOffset);
                currentDir = Vector3.Reflect(currentDir, hit.normal);
            }
            else
            {
                line.SetPosition(i + 1, currentPos + currentDir * maxDistance);
                Debug.DrawRay(currentPos, currentDir * maxDistance, Color.red, 0.1f);
                break;
            }
        }
    }
}

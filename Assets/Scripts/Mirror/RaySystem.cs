using UnityEngine;

public class RaySystem : MonoBehaviour
{
    [SerializeField] private int numberOfRays = 10; // Added default value
    [SerializeField] private LineRenderer line;
    [SerializeField] private float maxRayDistance = 100f; // Made configurable

    private void Start()
    {
        if (line == null)
        {
            line = GetComponent<LineRenderer>();
        }
        line.positionCount = numberOfRays + 1;
    }

    private void Update()
    {
        // Clear previous positions
        for (int i = 0; i < line.positionCount; i++)
        {
            line.SetPosition(i, Vector3.zero);
        }

        // Set first position to mirror's position
        line.SetPosition(0, transform.position);

        // Cast the initial ray
        CastRay(transform.position, transform.forward);
    }

    private void CastRay(Vector3 rayPos, Vector3 rayDir)
    {
        for (var i = 0; i < numberOfRays; i++)
        {
            var ray = new Ray(rayPos, rayDir);
            if (Physics.Raycast(ray, out var rayHit, maxRayDistance))
            {
                line.SetPosition(i + 1, rayHit.point);
                Debug.DrawLine(rayPos, rayHit.point, Color.red);

                // Update for next ray
                rayPos = rayHit.point;
                rayDir = Vector3.Reflect(rayDir, rayHit.normal); // Fixed reflection
            }
            else
            {
                line.SetPosition(i + 1, rayPos + rayDir * maxRayDistance);
                Debug.DrawRay(rayPos, rayDir * maxRayDistance, Color.blue);
                break;
            }
        }
    }
}
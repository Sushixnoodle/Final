using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LineRenderer))]
public class TargetScript : MonoBehaviour
{
    [Header("Core Settings")]
    [Tooltip("Maximum laser bounces")]
    public int maxBounces = 10;

    [Tooltip("Max distance per segment")]
    public float maxDistance = 100f;

    [Tooltip("ONLY reflects off these layers")]
    public LayerMask mirrorLayers; // Set to ONLY your Mirror layer

    [Tooltip("Prevents self-collision")]
    public float surfaceOffset = 0.01f;

    [Header("Visual Feedback")]
    public Color activeColor = Color.red;
    public Color blockedColor = Color.gray;

    private LineRenderer line;
    private RaycastHit hit;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = true;
        line.positionCount = 1; // Start with just origin point
    }

    private void Update()
    {
        ShootLaser();
    }

    private void ShootLaser()
    {
        // Reset laser
        line.positionCount = 1;
        line.SetPosition(0, transform.position);
        line.startColor = line.endColor = activeColor;

        Vector3 currentPos = transform.position;
        Vector3 currentDir = transform.forward;

        for (int i = 0; i < maxBounces; i++)
        {
            if (Physics.Raycast(currentPos, currentDir, out hit, maxDistance))
            {
                // Add new point at hit location
                line.positionCount++;
                line.SetPosition(i + 1, hit.point);

                // Check if NOT a mirror
                if (!IsMirror(hit.collider))
                {
                    // NON-MIRROR: STOP LASER
                    line.startColor = line.endColor = blockedColor;
                    Debug.Log($"Laser blocked by {hit.collider.name}", hit.collider.gameObject);
                    return; // Complete exit
                }

                // MIRROR: Reflect
                currentPos = hit.point + (hit.normal * surfaceOffset);
                currentDir = Vector3.Reflect(currentDir, hit.normal);
            }
            else
            {
                // No hit - draw max distance
                line.positionCount++;
                line.SetPosition(i + 1, currentPos + currentDir * maxDistance);
                return;
            }
        }
    }

    private bool IsMirror(Collider collider)
    {
        return ((1 << collider.gameObject.layer) & mirrorLayers) != 0;
    }
}
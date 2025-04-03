using UnityEngine;

public class LaserEmitter : MonoBehaviour
{
    [Header("Laser Settings")]
    public LineRenderer laserLine;
    public LayerMask reflectionLayers;
    public float maxDistance = 100f;
    public int maxBounces = 10;

    private void Start()
    {
        if (laserLine == null) laserLine = GetComponent<LineRenderer>();
        laserLine.positionCount = maxBounces + 1;
    }

    private void Update()
    {
        UpdateLaser();
    }

    private void UpdateLaser()
    {
        // Reset all positions
        for (int i = 0; i < laserLine.positionCount; i++)
            laserLine.SetPosition(i, Vector3.zero);

        Vector3 currentPos = transform.position;
        Vector3 currentDir = transform.forward;

        laserLine.SetPosition(0, currentPos);

        for (int i = 0; i < maxBounces; i++)
        {
            if (Physics.Raycast(currentPos, currentDir, out RaycastHit hit, maxDistance, reflectionLayers))
            {
                laserLine.SetPosition(i + 1, hit.point);

                // Notify mirror if it's hit
                MirrorController mirror = hit.collider.GetComponent<MirrorController>();
                if (mirror != null)
                    mirror.ReceiveLaser(currentDir, hit.point);

                // Prepare next bounce
                currentPos = hit.point;
                currentDir = Vector3.Reflect(currentDir, hit.normal);
            }
            else
            {
                laserLine.SetPosition(i + 1, currentPos + currentDir * maxDistance);
                break;
            }
        }
    }
}
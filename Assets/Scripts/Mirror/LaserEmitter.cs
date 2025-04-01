using UnityEngine;

public class LaserEmitter : MonoBehaviour
{
    public LineRenderer laserLine;
    public LayerMask reflectionLayers;
    public float maxDistance = 100f;
    public Color laserColor = Color.red;

    private void Start()
    {
        laserLine.startColor = laserLine.endColor = laserColor;
        laserLine.startWidth = laserLine.endWidth = 0.1f;
    }

    private void Update()
    {
        ShootLaser();
    }

    private void ShootLaser()
    {
        laserLine.positionCount = 2;
        laserLine.SetPosition(0, transform.position);

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxDistance, reflectionLayers))
        {
            laserLine.SetPosition(1, hit.point);

            // Check if we hit a mirror
            MirrorController mirror = hit.collider.GetComponent<MirrorController>();
            if (mirror != null)
            {
                mirror.ReceiveLaser(transform.forward);
            }
        }
        else
        {
            laserLine.SetPosition(1, transform.position + transform.forward * maxDistance);
        }
    }
}
using UnityEngine;

public class MirrorController : MonoBehaviour
{
    [Header("Settings")]
    public float rotationSpeed = 90f;
    public bool isLocked = false;
    public Color laserHitColor = Color.cyan;

    // Called when laser hits the mirror (for RaySystem)
    public void OnLaserHit()
    {
        // Visual feedback
        GetComponent<Renderer>().material.color = laserHitColor;
    }

    // Called when laser hits the mirror (for LaserEmitter)
    public void ReceiveLaser(Vector3 direction, Vector3 hitPoint)
    {
        OnLaserHit(); // Reuse the same visual feedback
    }

    private void Update()
    {
        if (!isLocked && Input.GetMouseButton(0))
        {
            HandleRotation();
        }
    }

    private void HandleRotation()
    {
        float rotationAmount = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotationAmount, 0);
    }
}
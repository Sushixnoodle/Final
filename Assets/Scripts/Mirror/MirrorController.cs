using UnityEngine;

public class MirrorController : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 90f; // Degrees per second
    public float snapAngle = 45f; // Snap to nearest X degrees
    public bool isLocked = false; // Can player rotate this mirror?

    [Header("Reflection")]
    public LineRenderer reflectionLine;
    public LayerMask reflectionLayers;
    public float maxReflectionDistance = 50f;

    private Vector3 incomingRayDirection;
    private bool isReflecting;

    private void Update()
    {
        if (!isLocked && Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
            {
                RotateMirror();
            }
        }

        if (isReflecting)
        {
            UpdateReflection();
        }
    }

    private void RotateMirror()
    {
        float rotationAmount = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotationAmount, 0);

        // Snap to nearest angle
        float currentAngle = transform.eulerAngles.y;
        float snappedAngle = Mathf.Round(currentAngle / snapAngle) * snapAngle;
        transform.rotation = Quaternion.Euler(0, snappedAngle, 0);
    }

    public void ReceiveLaser(Vector3 direction)
    {
        incomingRayDirection = direction;
        isReflecting = true;
        reflectionLine.enabled = true;
        UpdateReflection();
    }

    private void UpdateReflection()
    {
        Vector3 reflectionDirection = Vector3.Reflect(incomingRayDirection.normalized, transform.forward);
        Vector3 startPoint = transform.position + transform.forward * 0.1f; // Slightly in front

        reflectionLine.positionCount = 2;
        reflectionLine.SetPosition(0, startPoint);

        if (Physics.Raycast(startPoint, reflectionDirection, out RaycastHit hit, maxReflectionDistance, reflectionLayers))
        {
            reflectionLine.SetPosition(1, hit.point);

            // Check if we hit another mirror
            MirrorController otherMirror = hit.collider.GetComponent<MirrorController>();
            if (otherMirror != null)
            {
                otherMirror.ReceiveLaser(reflectionDirection);
            }
        }
        else
        {
            reflectionLine.SetPosition(1, startPoint + reflectionDirection * maxReflectionDistance);
        }
    }

    public void StopReflection()
    {
        isReflecting = false;
        reflectionLine.enabled = false;
    }
}
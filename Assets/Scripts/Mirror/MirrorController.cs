using UnityEngine;

public class MirrorController : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private bool limitRotation = false;
    [SerializeField] private Vector2 rotationLimits = new Vector2(-45f, 45f);

    private Vector3 initialRotation;
    private bool isBeingRotated = false;

    void Start()
    {
        initialRotation = transform.eulerAngles;
    }

    void OnMouseDown()
    {
        isBeingRotated = true;
    }

    void OnMouseUp()
    {
        isBeingRotated = false;
    }

    void Update()
    {
        if (isBeingRotated && Input.GetMouseButton(0))
        {
            float rotationAmount = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            RotateMirror(rotationAmount);
        }
    }

    void RotateMirror(float amount)
    {
        Vector3 newRotation = transform.eulerAngles + new Vector3(0, amount, 0);

        if (limitRotation)
        {
            float currentYRotation = NormalizeAngle(newRotation.y);
            float minY = NormalizeAngle(initialRotation.y + rotationLimits.x);
            float maxY = NormalizeAngle(initialRotation.y + rotationLimits.y);

            newRotation.y = Mathf.Clamp(currentYRotation, minY, maxY);
        }

        transform.eulerAngles = newRotation;
    }

    float NormalizeAngle(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }
}
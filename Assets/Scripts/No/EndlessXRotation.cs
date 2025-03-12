using UnityEngine;

public class EndlessXRotation : MonoBehaviour
{
    // Speed of rotation in degrees per second
    public float rotationSpeed = 100f;

    void Update()
    {
        // Rotate the object around its Y-axis
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
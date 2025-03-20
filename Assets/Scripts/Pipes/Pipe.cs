using UnityEngine;

public class Pipe : MonoBehaviour
{
    public Quaternion targetRotation;
    public float threshold = 5f; // Allow a small margin of error

    public bool IsCorrect()
    {
        float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);
        return angleDifference <= threshold;
    }
}
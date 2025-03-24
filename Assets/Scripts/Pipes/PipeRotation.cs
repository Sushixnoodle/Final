using UnityEngine;

public class PipeRotation : MonoBehaviour
{
    public AudioClip rotateSound;
    private AudioSource audioSource;
    public Transform correctPipeReference;

    // Rotation system
    private const float FIXED_Y = 90f;
    private const float FIXED_Z = -90f;
    private const float ROTATION_STEP = 15f;
    private int currentStep = 0;

    // Alignment tolerance
    private const float ANGLE_TOLERANCE = 0.3f; // Increased from 0.1f to 0.3f

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = rotateSound;
        InitializeRotation();
    }

    private void InitializeRotation()
    {
        float initialX = NormalizeAngle(transform.eulerAngles.x);
        currentStep = Mathf.RoundToInt(initialX / ROTATION_STEP);
        ApplyExactRotation();
    }

    private void OnMouseDown()
    {
        RotatePipe();
        PuzzleManager.Instance?.CheckPuzzleCompletion();
    }

    private void RotatePipe()
    {
        currentStep++;
        ApplyExactRotation();

        if (rotateSound != null)
            audioSource.Play();
    }

    private void ApplyExactRotation()
    {
        float exactX = (currentStep * ROTATION_STEP) % 360f;
        transform.rotation = Quaternion.Euler(exactX, FIXED_Y, FIXED_Z);
    }

    public bool IsCorrectlyAligned()
    {
        if (correctPipeReference == null)
        {
            Debug.LogError($"{name}: Missing correct pipe reference!");
            return false;
        }

        float currentAngle = (currentStep * ROTATION_STEP) % 360f;
        float targetAngle = NormalizeAngle(correctPipeReference.eulerAngles.x);
        float angleDiff = Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle));

        // Check if within tolerance (0.3 degrees)
        bool isAligned = angleDiff <= ANGLE_TOLERANCE;

        // Special case for 180°-0° equivalence
        if (!isAligned && Mathf.Abs(angleDiff - 180f) <= ANGLE_TOLERANCE)
        {
            isAligned = true;
        }

        if (!isAligned)
        {
            Debug.Log($"{name} misaligned by {angleDiff}° (Current: {currentAngle}°, Target: {targetAngle}°)");
        }

        return isAligned;
    }

    private float NormalizeAngle(float angle)
    {
        angle = angle % 360f;
        return angle < 0 ? angle + 360f : angle;
    }

    [ContextMenu("Print Alignment Info")]
    private void PrintAlignmentInfo()
    {
        Debug.Log($"{name} aligned: {IsCorrectlyAligned()}");
    }
}
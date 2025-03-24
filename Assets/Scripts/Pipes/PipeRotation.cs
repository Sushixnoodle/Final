using UnityEngine;

public class PipeRotation : MonoBehaviour
{
    public AudioClip rotateSound;
    private AudioSource audioSource;
    public Transform correctPipeReference;

    // Exact rotation constraints
    private const float FIXED_Y = 90f;
    private const float FIXED_Z = -90f;
    private const float ROTATION_STEP = 15f;
    private int currentStep = 0;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = rotateSound;
        InitializeRotation();
    }

    private void InitializeRotation()
    {
        // Calculate initial step based on current rotation
        float initialX = NormalizeAngle(transform.eulerAngles.x);
        currentStep = Mathf.RoundToInt(initialX / ROTATION_STEP);
        ApplyExactRotation();
    }

    private void OnMouseDown()
    {
        RotatePipe();
        PuzzleManager.Instance.CheckPuzzleCompletion();
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
        // Calculate exact X rotation without floating-point drift
        float exactX = (currentStep * ROTATION_STEP) % 360f;
        transform.rotation = Quaternion.Euler(exactX, FIXED_Y, FIXED_Z);
    }

    private float NormalizeAngle(float angle)
    {
        angle = angle % 360f;
        return angle < 0 ? angle + 360f : angle;
    }

    public bool IsCorrectlyAligned()
    {
        if (correctPipeReference == null)
        {
            Debug.LogError($"{name}: Missing correct pipe reference!");
            return false;
        }

        float currentX = NormalizeAngle(transform.eulerAngles.x);
        float correctX = NormalizeAngle(correctPipeReference.eulerAngles.x);
        float angleDiff = Mathf.Abs(Mathf.DeltaAngle(currentX, correctX));

        Debug.Log($"{name} Alignment - Current: {currentX}, Target: {correctX}, Diff: {angleDiff}");

        return angleDiff < 1.5f;
    }

    [ContextMenu("Force Check Alignment")]
    private void DebugAlignment()
    {
        Debug.Log($"{name} is {(IsCorrectlyAligned() ? "ALIGNED" : "MISALIGNED")}");
    }
}

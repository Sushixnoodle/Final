using UnityEngine;

public class PipeRotation : MonoBehaviour
{
    public AudioClip rotateSound; // Assign in Inspector
    private AudioSource audioSource;

    public Transform correctPipeReference; // Drag the correct pipe from the hidden area here

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = rotateSound;
    }

    private void OnMouseDown()
    {
        RotatePipe();
        PuzzleManager.Instance.CheckPuzzleCompletion();
    }

    private void RotatePipe()
    {
        transform.Rotate(0, 90, 0); // Rotate around the Y-axis (side to side)

        if (rotateSound != null)
        {
            audioSource.Play();
        }
    }

    public bool IsCorrectlyAligned()
    {
        if (correctPipeReference == null)
        {
            Debug.LogWarning($"{gameObject.name} has no correct pipe reference assigned!");
            return false;
        }

        return Mathf.Approximately(Mathf.Repeat(transform.eulerAngles.y, 360), Mathf.Repeat(correctPipeReference.eulerAngles.y, 360));
    }
}

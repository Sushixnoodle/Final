using UnityEngine;

public class CutsceneCameraLook : MonoBehaviour
{
    [Header("Camera Settings")]
    public float mouseSensitivity = 100f;
    public Transform playerBody; // The parent object that will rotate horizontally

    [Header("Rotation Limits")]
    public bool clampVerticalRotation = true;
    public float minVerticalAngle = -80f;
    public float maxVerticalAngle = 80f;

    private float xRotation = 0f; // Tracks vertical rotation
    private bool isCutsceneActive = true;

    void Start()
    {
        // Lock and hide cursor during cutscene
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize rotation to current orientation
        xRotation = transform.localEulerAngles.x;
    }

    void Update()
    {
        if (isCutsceneActive)
        {
            HandleCameraRotation();
        }
    }

    private void HandleCameraRotation()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertical rotation (up/down) - applied to the camera itself
        xRotation -= mouseY;

        if (clampVerticalRotation)
        {
            xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);
        }

        // Apply vertical rotation to camera
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal rotation (left/right) - applied to the player body or camera parent
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }
        else
        {
            // If no player body is assigned, rotate the camera parent instead
            transform.parent.Rotate(Vector3.up * mouseX);
        }
    }

    public void StartCutscene()
    {
        isCutsceneActive = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void EndCutscene()
    {
        isCutsceneActive = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
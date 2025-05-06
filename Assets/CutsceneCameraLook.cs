using UnityEngine;

public class CutsceneCameraLook : MonoBehaviour
{
    [Header("Look Settings")]
    public float mouseSensitivity = 100f;
    public bool invertY = false;

    [Header("Rotation Limits")]
    public bool clampVerticalRotation = true;
    public float minVerticalAngle = -80f;
    public float maxVerticalAngle = 80f;

    private float _xRotation = 0f; // Tracks vertical (up/down) rotation
    private float _yRotation = 0f; // Tracks horizontal (left/right) rotation
    private bool _isCutsceneActive = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize current rotation
        Vector3 currentRot = transform.localEulerAngles;
        _xRotation = currentRot.x;
        _yRotation = currentRot.y;
    }

    void Update()
    {
        if (_isCutsceneActive)
            HandleCameraRotation();
    }

    private void HandleCameraRotation()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Horizontal rotation (left/right)
        _yRotation += mouseX;

        // Vertical rotation (up/down)
        _xRotation += (invertY ? 1 : -1) * mouseY;

        // Clamp vertical angle
        if (clampVerticalRotation)
            _xRotation = Mathf.Clamp(_xRotation, minVerticalAngle, maxVerticalAngle);

        // Apply rotation to camera
        transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
    }

    public void StartCutscene()
    {
        _isCutsceneActive = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void EndCutscene()
    {
        _isCutsceneActive = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
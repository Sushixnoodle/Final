using UnityEngine;
using UnityEngine.UI;

public class RotatePipe : MonoBehaviour
{
    public float rotationSpeed = 1000f; // Increased rotation speed
    public float maxDistanceFromCenter = 300f; // Increased maximum distance
    public RectTransform dotUI; // Reference to the dot UI
    public Image dotImage; // Reference to the Image component of the dot
    public Transform pipeChild; // Reference to the pipe child GameObject (if using empty parent)

    void Start()
    {
        Cursor.visible = false; // Hide the mouse cursor
    }

    void Update()
    {
        // Get the mouse position in screen coordinates
        Vector2 mousePosition = Input.mousePosition;

        // Calculate the center of the screen
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        // Calculate the distance between the mouse and the center of the screen
        float distance = Vector2.Distance(mousePosition, screenCenter);

        // Check if the mouse is within the allowed distance and the left mouse button is pressed
        if (distance <= maxDistanceFromCenter)
        {
            dotImage.color = Color.green; // Change dot color to green when in range

            if (Input.GetMouseButton(0))
            {
                // Calculate rotation based on mouse movement
                float mouseX = Input.GetAxis("Mouse X");
                float rotation = mouseX * rotationSpeed * Time.deltaTime * 20f; // Increased sensitivity

                // Debug: Log the rotation value
                Debug.Log("Rotating pipe: " + rotation);

                // Rotate the pipe (or pipe child)
                if (pipeChild != null)
                {
                    // Rotate the pipe child (if using empty parent)
                    pipeChild.Rotate(0, rotation, 0, Space.Self); // Use Space.Self for local rotation
                    Debug.Log("Pipe Child Rotation: " + pipeChild.rotation);
                }
                else
                {
                    // Rotate the pipe directly (if no empty parent)
                    transform.Rotate(0, rotation, 0, Space.Self); // Use Space.Self for local rotation
                    Debug.Log("Pipe Rotation: " + transform.rotation);
                }
            }
        }
        else
        {
            dotImage.color = Color.red; // Change dot color to red when out of range
        }

        // Update the dot UI position to follow the mouse
        dotUI.position = mousePosition;
    }
}
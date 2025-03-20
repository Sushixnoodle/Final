using UnityEngine;
using UnityEngine.UI; // Required for UI components
using UnityEngine.SceneManagement;

public class EnableChildOnClick : MonoBehaviour
{
    public GameObject childObject; // Assign the child GameObject in the Inspector
    public Image uiImage;          // Assign the UI Image in the Inspector
    public string nextSceneName;   // Name of the next scene to load
    private float holdTimer = 0f;  // Timer to track how long the button is held
    private bool isHolding = false; // Flag to check if the mouse is held over the object
    private Animator animator;     // Reference to the Animator component

    private void Start()
    {
        // Get the Animator component from the child object
        if (childObject != null)
        {
            animator = childObject.GetComponent<Animator>();
        }
    }

    private void Update()
    {
        // Check if the left mouse button is held down
        if (Input.GetMouseButton(0)) // 0 is the left mouse button
        {
            // Create a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits the parent object
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject) // Check if the hit object is the parent
                {
                    if (!isHolding)
                    {
                        // Reset the animation to the beginning
                        if (animator != null)
                        {
                            animator.Play("USING THISZ", -1, 0f); // Replace with your animation state name
                        }

                        // Enable the child object
                        if (childObject != null)
                        {
                            childObject.SetActive(true);
                        }

                        // Enable the UI Image
                        if (uiImage != null)
                        {
                            uiImage.enabled = true;
                        }

                        isHolding = true;
                    }

                    // Increment the hold timer
                    holdTimer += Time.deltaTime;

                    // Check if the timer has reached 5 seconds
                    if (holdTimer >= 5f)
                    {
                        // Load the next scene
                        if (!string.IsNullOrEmpty(nextSceneName))
                        {
                            SceneManager.LoadScene(nextSceneName);
                        }
                    }
                }
            }
            else
            {
                ResetTimer();
            }
        }
        else
        {
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        // Reset the timer and disable the child object and UI Image if the mouse is not held over the object
        isHolding = false;
        holdTimer = 0f;

        if (childObject != null)
        {
            childObject.SetActive(false);
        }

        if (uiImage != null)
        {
            uiImage.enabled = false;
        }
    }
}
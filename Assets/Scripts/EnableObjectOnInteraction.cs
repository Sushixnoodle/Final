using UnityEngine;

public class EnableObjectOnInteraction : MonoBehaviour
{
    public string interactableTag = "Interactable"; // Tag for interactable objects
    public Transform player; // Reference to the player's transform
    public float interactionDistance = 3f; // Maximum distance allowed for interaction

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse click
        {
            Debug.Log("Left mouse button clicked!"); // Debug: Click detected

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Ray from camera to mouse position

            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) // Check if ray hits something
            {
                Debug.Log("Raycast hit: " + hit.collider.gameObject.name); // Debug: Print object hit

                if (hit.collider.CompareTag(interactableTag)) // Check if the object has the correct tag
                {
                    float distance = Vector3.Distance(player.position, hit.collider.transform.position);

                    if (distance <= interactionDistance) // Check if player is close enough
                    {
                        Transform targetObject = hit.collider.transform.GetChild(0); // Assuming first child is the actual object
                        bool newState = !targetObject.gameObject.activeSelf;
                        targetObject.gameObject.SetActive(newState); // Toggle object state

                        Debug.Log("Toggled " + targetObject.name + " to " + newState); // Debug: Show new state
                    }
                    else
                    {
                        Debug.Log("Too far away! Move closer to interact.");
                    }
                }
                else
                {
                    Debug.Log("Hit object does NOT have the 'Interactable' tag.");
                }
            }
            else
            {
                Debug.Log("Raycast did NOT hit any object.");
            }
        }
    }
}

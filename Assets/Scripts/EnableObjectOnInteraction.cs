using UnityEngine;
using System.Collections;

public class EnableObjectOnInteraction : MonoBehaviour
{
    public string interactableTag = "Interactable"; // Tag for interactable objects
    public Transform player; // Reference to the player's transform
    public float interactionDistance = 3f; // Maximum distance allowed for interaction
    public GameObject[] specificInteractables; // Array of specific interactable objects to track
    public GameObject[] newObjectsToEnable; // Array of new objects to enable when condition is met

    private int enabledCount = 0; // Counter for enabled specific interactables

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

                        // Check if the toggled object is one of the specific interactables
                        if (System.Array.Exists(specificInteractables, obj => obj == targetObject.gameObject))
                        {
                            enabledCount += newState ? 1 : -1; // Update the enabled count
                        }

                        // Check if all three specific interactables are enabled
                        if (enabledCount == 3)
                        {
                            StartCoroutine(HandleObjectTransition(2f, 2.5f)); // Start coroutine with 2s and 2.5s delays
                            enabledCount = 0; // Reset the counter
                        }
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

    private IEnumerator HandleObjectTransition(float disableDelay, float enableDelay)
    {
        // Wait for 2 seconds before disabling the previous objects
        yield return new WaitForSeconds(disableDelay);
        DisableAllInteractables();

        // Wait for an additional 0.5 seconds (total 2.5 seconds) before enabling the new objects
        yield return new WaitForSeconds(enableDelay - disableDelay);
        EnableNewObjects();
    }

    private void DisableAllInteractables()
    {
        GameObject[] interactables = GameObject.FindGameObjectsWithTag(interactableTag);
        foreach (GameObject interactable in interactables)
        {
            interactable.transform.GetChild(0).gameObject.SetActive(false);
        }
        Debug.Log("All interactable objects have been disabled.");
    }

    private void EnableNewObjects()
    {
        foreach (GameObject newObject in newObjectsToEnable)
        {
            newObject.SetActive(true);
        }
        Debug.Log("New objects have been enabled.");
    }
}

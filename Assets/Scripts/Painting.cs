using UnityEngine;
using UnityEngine.UI;

public class ObjectInteraction : MonoBehaviour
{
    public GameObject panel; // Assign the UI panel in the inspector
    public float interactionDistance = 3f; // Distance within which interaction is possible
    private bool isPanelActive = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button click
        {
            TryInteract();
        }

        // Automatically disable the panel if the player is out of range
        if (isPanelActive && Vector3.Distance(transform.position, Camera.main.transform.position) > interactionDistance)
        {
            TogglePanel();
        }
    }

    void TryInteract()
    {
        // Check distance between player and this object
        if (Vector3.Distance(transform.position, Camera.main.transform.position) <= interactionDistance)
        {
            TogglePanel();
        }
    }

    void TogglePanel()
    {
        isPanelActive = !isPanelActive;
        panel.SetActive(isPanelActive);
    }
}
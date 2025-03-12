using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedTorchesInteractable : MonoBehaviour
{
    public string interactableTag = "InteractableTwo"; // Tag for the interactable parent object
    public float interactionDistance = 3f; // Maximum distance allowed for interaction
    public Transform player; // Reference to the player's transform
    public AudioClip enableSound; // Sound to play when the object is enabled
    private AudioSource audioSource; // AudioSource component to play the sound

    void Start()
    {
        // Ensure there's an AudioSource component on this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // If no AudioSource exists, add one
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits something
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // Check if the hit object has the correct tag
                if (hit.collider.CompareTag(interactableTag))
                {
                    // Check if the player is within interaction distance
                    float distance = Vector3.Distance(player.position, hit.collider.transform.position);
                    if (distance <= interactionDistance)
                    {
                        // Get the child object (assuming there's only one child)
                        Transform childObject = hit.collider.transform.GetChild(0);

                        // Toggle the child object's active state
                        bool newState = !childObject.gameObject.activeSelf;
                        childObject.gameObject.SetActive(newState);

                        // Play sound if the object is enabled
                        if (newState && enableSound != null)
                        {
                            audioSource.PlayOneShot(enableSound);
                        }

                        // Debug log to confirm the toggle
                        Debug.Log("Toggled " + childObject.name + " to " + newState);
                    }
                    else
                    {
                        Debug.Log("Too far away! Move closer to interact.");
                    }
                }
            }
        }
    }
}

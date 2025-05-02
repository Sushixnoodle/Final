using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class EnableObjectOnInteraction : MonoBehaviour
{
    public string interactableTag = "Interactable"; // Tag for interactable objects
    public Transform player; // Reference to the player's transform
    public float interactionDistance = 3f; // Maximum distance allowed for interaction
    public GameObject[] specificInteractables; // Array of specific interactable objects to track
    public GameObject[] newObjectsToEnable; // Array of new objects to enable when condition is met

    public AudioClip enableSound; // Sound to play when an object is enabled
    public AudioClip disableSound; // Sound to play when an object is disabled
    public AudioClip newObjectsSound; // Sound to play when the new objects are enabled

    private AudioSource audioSource; // Reference to the AudioSource component
    private int enabledCount = 0; // Counter for enabled specific interactables

    void Start()
    {
        // Get or add an AudioSource component to this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

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

                        // Play sound based on the new state
                        if (newState)
                        {
                            PlaySound(enableSound); // Play enable sound
                        }
                        else
                        {
                            PlaySound(disableSound); // Play disable sound
                        }

                        // Check if the toggled object is one of the specific interactables
                        if (System.Array.Exists(specificInteractables, obj => obj == targetObject.gameObject))
                        {
                            enabledCount += newState ? 1 : -1; // Update the enabled count
                        }

                        // Check if all three specific interactables are enabled and NO other interactables are enabled
                        if (enabledCount == 3 && AreOnlySpecificInteractablesEnabled())
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

    private bool AreOnlySpecificInteractablesEnabled()
    {
        // Check if ONLY the specific interactables are enabled
        GameObject[] allInteractables = GameObject.FindGameObjectsWithTag(interactableTag);
        foreach (GameObject interactable in allInteractables)
        {
            // If any interactable is enabled and is NOT one of the specific interactables, return false
            if (interactable.transform.GetChild(0).gameObject.activeSelf &&
                !System.Array.Exists(specificInteractables, obj => obj == interactable.transform.GetChild(0).gameObject))
            {
                Debug.Log("Another interactable is enabled: " + interactable.name);
                return false;
            }
        }
        return true; // Only specific interactables are enabled
    }

    private IEnumerator HandleObjectTransition(float disableDelay, float enableDelay)
    {
        // Wait for 2 seconds before disabling the previous objects
        yield return new WaitForSeconds(disableDelay);
        DisableAllInteractables();

        // Wait for an additional 0.5 seconds (total 2.5 seconds) before enabling the new objects
        yield return new WaitForSeconds(enableDelay - disableDelay);
        EnableNewObjects();

        // Play sound when new objects are enabled
        PlaySound(newObjectsSound);

        // Wait for x seconds after enabling the new objects
        yield return new WaitForSeconds(1.5f);

        // Change the scene
        ChangeScene();
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

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip); // Play the specified sound
        }
    }
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            yield return null;
        }
    }
    private void ChangeScene()
    {

         StartCoroutine(LoadSceneAsync("AfterTorch"));
        //Debug.Log("Scene changed to: YourSceneName");
    }
}
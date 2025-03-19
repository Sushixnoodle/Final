using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickAndHoldUI : MonoBehaviour
{
    public GameObject targetObject; // The object that needs to be clicked and held
    public GameObject initialUI;    // The initial UI that is enabled
    public GameObject panelUI;      // The panel that will be enabled after holding
    public AudioClip holdSoundClip; // The sound that will play while holding
    public AudioClip panelSoundClip; // The sound that will play during the panel
    public string nextSceneName;   // The name of the next scene to load

    private float holdDuration = 5f; // Duration to hold the mouse click
    private float panelDuration = 7f; // Duration the panel is enabled
    private float soundDuration = 3f; // Duration the sound plays during the panel
    private float elapsedTime = 0f;
    private bool isHolding = false;
    private AudioSource audioSource;
    private Vector3 initialScale; // Initial scale of the target object

    void Start()
    {
        // Ensure the panel is disabled at the start
        if (panelUI != null)
        {
            panelUI.SetActive(false);
        }

        // Ensure the initial UI is enabled at the start
        if (initialUI != null)
        {
            initialUI.SetActive(true);
        }

        // Add an AudioSource component if not already present
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Store the initial scale of the target object
        if (targetObject != null)
        {
            initialScale = targetObject.transform.localScale;
        }
    }

    void Update()
    {
        // Check if the left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits the target object
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == targetObject)
            {
                isHolding = true;
                elapsedTime = 0f;

                // Play the hold sound
                if (holdSoundClip != null && audioSource != null)
                {
                    audioSource.clip = holdSoundClip;
                    audioSource.loop = true; // Loop the sound while holding
                    audioSource.Play();
                }
            }
        }

        // Check if the left mouse button is held down
        if (Input.GetMouseButton(0) && isHolding)
        {
            elapsedTime += Time.deltaTime;

            // Scale the object gradually
            if (targetObject != null)
            {
                float scaleFactor = 1 + (elapsedTime / holdDuration); // Increase scale over time
                targetObject.transform.localScale = initialScale * scaleFactor;
            }

            // If the hold duration is reached
            if (elapsedTime >= holdDuration)
            {
                isHolding = false;
                OnHoldComplete();
            }
        }

        // Check if the left mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            isHolding = false;

            // Stop the hold sound
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            // Reset the object scale
            if (targetObject != null)
            {
                targetObject.transform.localScale = initialScale;
            }
        }
    }

    void OnHoldComplete()
    {
        // Stop the hold sound
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // Disable the initial UI
        if (initialUI != null)
        {
            initialUI.SetActive(false);
        }

        // Enable the panel UI
        if (panelUI != null)
        {
            panelUI.SetActive(true);
        }

        // Play the panel sound
        if (panelSoundClip != null && audioSource != null)
        {
            audioSource.clip = panelSoundClip;
            audioSource.loop = false; // Do not loop the panel sound
            audioSource.Play();
        }

        // Start the coroutine to handle the panel duration
        StartCoroutine(PanelSequence());
    }

    System.Collections.IEnumerator PanelSequence()
    {
        // Wait for the sound duration
        yield return new WaitForSeconds(soundDuration);

        // Stop the panel sound
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // Wait for the remaining panel duration
        yield return new WaitForSeconds(panelDuration - soundDuration);

        // Enable the initial UI again
        if (initialUI != null)
        {
            initialUI.SetActive(true);
        }

        // Load the next scene
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
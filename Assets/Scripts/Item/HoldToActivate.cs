using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HoldToActivate : MonoBehaviour
{
    [Header("Hold Settings")]
    [SerializeField] private float holdDuration = 3f;
    [SerializeField] private GameObject objectToEnableDuringHold;
    [SerializeField] private AudioSource holdSound;

    [Header("Panel Settings")]
    [SerializeField] private Image fadePanel;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private AudioSource completeSound;

    [Header("Final Objects")]
    [SerializeField] private GameObject objectToDisableAfterLoad;
    [SerializeField] private GameObject objectToEnableAfterLoad;
    [SerializeField] private float waitBeforeSwitch = 4f;

    private float holdTimer = 0f;
    private bool isHolding = false;
    private bool sequenceCompleted = false;

    private void Start()
    {
        // Initialize panel
        if (fadePanel != null)
        {
            Color c = fadePanel.color;
            c.a = 0f;
            fadePanel.color = c;
            fadePanel.gameObject.SetActive(false);
        }

        // Initialize objects
        if (objectToEnableDuringHold != null)
            objectToEnableDuringHold.SetActive(false);

        if (objectToDisableAfterLoad != null)
            objectToDisableAfterLoad.SetActive(true);

        if (objectToEnableAfterLoad != null)
            objectToEnableAfterLoad.SetActive(false);
    }

    private void Update()
    {
        if (sequenceCompleted) return;

        // Check for mouse button down
        if (Input.GetMouseButtonDown(0))
        {
            StartHold();
        }

        // Check for mouse button held
        if (isHolding && Input.GetMouseButton(0))
        {
            holdTimer += Time.deltaTime;

            // Check if hold duration reached
            if (holdTimer >= holdDuration)
            {
                CompleteHold();
            }
        }
        // Check for mouse button release
        else if (isHolding && Input.GetMouseButtonUp(0))
        {
            CancelHold();
        }
    }

    private void StartHold()
    {
        isHolding = true;
        holdTimer = 0f;

        // Enable object during hold
        if (objectToEnableDuringHold != null)
            objectToEnableDuringHold.SetActive(true);

        // Play hold sound
        if (holdSound != null)
            holdSound.Play();
    }

    private void CancelHold()
    {
        isHolding = false;
        holdTimer = 0f;

        // Disable object during hold
        if (objectToEnableDuringHold != null)
            objectToEnableDuringHold.SetActive(false);

        // Stop hold sound
        if (holdSound != null)
            holdSound.Stop();
    }

    private void CompleteHold()
    {
        sequenceCompleted = true;
        isHolding = false;

        // Disable hold object
        if (objectToEnableDuringHold != null)
            objectToEnableDuringHold.SetActive(false);

        // Stop hold sound
        if (holdSound != null)
            holdSound.Stop();

        // Play complete sound
        if (completeSound != null)
            completeSound.Play();

        // Start fade in coroutine
        StartCoroutine(FadeInPanel());
    }

    private IEnumerator FadeInPanel()
    {
        if (fadePanel == null) yield break;

        fadePanel.gameObject.SetActive(true);
        Color c = fadePanel.color;
        float startAlpha = c.a;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, 1f, timer / fadeDuration);
            fadePanel.color = c;
            yield return null;
        }

        c.a = 1f;
        fadePanel.color = c;

        // Wait before switching objects
        yield return new WaitForSeconds(waitBeforeSwitch);

        // Switch objects
        if (objectToDisableAfterLoad != null)
            objectToDisableAfterLoad.SetActive(false);

        if (objectToEnableAfterLoad != null)
            objectToEnableAfterLoad.SetActive(true);
    }
}
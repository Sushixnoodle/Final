using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HoldToActivate : MonoBehaviour
{
    [Header("Hold Settings")]
    [SerializeField] private float holdDuration = 3f;
    [SerializeField] private GameObject holdVisualObject;
    [SerializeField] private AudioSource holdSound;

    [Header("Fade Panel Settings")]
    [SerializeField] private Image fadePanel;
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float panelDisplayTime = 5f;
    [SerializeField] private float fadeOutDuration = 1f;
    [SerializeField] private AudioSource panelCompleteSound;

    [Header("Objects to Disable")]
    [SerializeField] private List<GameObject> objectsToDisable = new List<GameObject>();

    [Header("Objects to Enable")]
    [SerializeField] private List<GameObject> objectsToEnable = new List<GameObject>();

    private float holdTimer = 0f;
    private bool isHolding = false;
    private bool sequenceStarted = false;

    private void Start()
    {
        // Initialize all elements
        if (fadePanel != null)
        {
            fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 0);
            fadePanel.gameObject.SetActive(false);
        }

        if (holdVisualObject != null) holdVisualObject.SetActive(false);

        // Initialize all objects to disable
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null) obj.SetActive(true);
        }

        // Initialize all objects to enable
        foreach (GameObject obj in objectsToEnable)
        {
            if (obj != null) obj.SetActive(false);
        }
    }

    private void Update()
    {
        if (sequenceStarted) return;

        if (Input.GetMouseButtonDown(0))
        {
            StartHolding();
        }

        if (isHolding)
        {
            if (Input.GetMouseButton(0))
            {
                holdTimer += Time.deltaTime;
                if (holdTimer >= holdDuration)
                {
                    StartPanelSequence();
                }
            }
            else
            {
                CancelHolding();
            }
        }
    }

    private void StartHolding()
    {
        isHolding = true;
        holdTimer = 0f;
        if (holdVisualObject != null) holdVisualObject.SetActive(true);
        if (holdSound != null) holdSound.Play();
    }

    private void CancelHolding()
    {
        isHolding = false;
        if (holdVisualObject != null) holdVisualObject.SetActive(false);
        if (holdSound != null) holdSound.Stop();
    }

    private void StartPanelSequence()
    {
        sequenceStarted = true;
        StartCoroutine(PanelSequence());
    }

    private IEnumerator PanelSequence()
    {
        // Phase 1: Fade in panel
        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(true);
            float timer = 0f;

            while (timer < fadeInDuration)
            {
                timer += Time.deltaTime;
                fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b,
                                         Mathf.Lerp(0, 1, timer / fadeInDuration));
                yield return null;
            }
        }

        // Phase 2: Panel fully visible
        if (holdSound != null) holdSound.Stop();
        if (holdVisualObject != null) holdVisualObject.SetActive(false);
        if (panelCompleteSound != null) panelCompleteSound.Play();

        // Disable all objects in the disable list
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null) obj.SetActive(false);
        }

        // Enable all objects in the enable list
        foreach (GameObject obj in objectsToEnable)
        {
            if (obj != null) obj.SetActive(true);
        }

        // Wait with panel fully visible
        yield return new WaitForSeconds(panelDisplayTime);

        // Phase 3: Fade out panel
        if (fadePanel != null)
        {
            float timer = 0f;
            while (timer < fadeOutDuration)
            {
                timer += Time.deltaTime;
                fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b,
                                         Mathf.Lerp(1, 0, timer / fadeOutDuration));
                yield return null;
            }

            // Phase 4: Panel fully hidden
            if (panelCompleteSound != null) panelCompleteSound.Stop();
            fadePanel.gameObject.SetActive(false);
        }

        // Destroy this script's GameObject after everything is done
        Destroy(gameObject);
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimatedText : MonoBehaviour
{
    [Header("UI Text References")]
    [Tooltip("Assign either a UI Text or TMP_Text here (only one)")]
    public Text uiText;
    public TMP_Text tmpText;

    [Header("Target Object")]
    [Tooltip("Object to watch. When it's destroyed, show the message.")]
    public GameObject targetObject;

    [Header("Message Settings")]
    [TextArea]
    public string message = "You picked up an item!";
    public float displayDuration = 5f;
    public float characterDelay = 0.05f;

    private bool messageShown = false;
    private Coroutine showRoutine;

    private void Start()
    {
        // Start with empty text
        if (uiText != null) uiText.text = "";
        if (tmpText != null) tmpText.text = "";
    }

    private void Update()
    {
        if (!messageShown && targetObject == null)
        {
            messageShown = true;
            ShowMessage(message);
        }
    }

    public void ShowMessage(string message)
    {
        if (showRoutine != null) StopCoroutine(showRoutine);
        showRoutine = StartCoroutine(AnimateText(message));
    }

    private IEnumerator AnimateText(string message)
    {
        string displayed = "";

        if (uiText != null) uiText.text = "";
        if (tmpText != null) tmpText.text = "";

        foreach (char c in message)
        {
            displayed += c;

            if (uiText != null) uiText.text = displayed;
            if (tmpText != null) tmpText.text = displayed;

            yield return new WaitForSeconds(characterDelay);
        }

        yield return new WaitForSeconds(displayDuration);

        if (uiText != null) uiText.text = "";
        if (tmpText != null) tmpText.text = "";
    }
}

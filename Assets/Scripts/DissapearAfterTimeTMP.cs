using UnityEngine;
using TMPro;

public class DisappearAfterTimeTMP : MonoBehaviour
{
    // Assign your TextMeshPro UI text in the Inspector if not attached to the same GameObject
    public TMP_Text uiText;

    // Time in seconds after which the text will disappear
    public float delay = 10f;

    void Start()
    {
        if (uiText == null)
        {
            uiText = GetComponent<TMP_Text>();
        }
        Invoke("HideText", delay);
    }

    void HideText()
    {
        if (uiText != null)
        {
            uiText.enabled = false;
        }
    }
}

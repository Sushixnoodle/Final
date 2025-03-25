using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class AnimateText : MonoBehaviour
{
    public float charactersPerSecond = 20f;
    public bool playOnEnable = true;
    public bool autoStart = true;

    private TMP_Text tmpComponent;
    private string fullText;
    private float delay;

    void Awake()
    {
        tmpComponent = GetComponent<TMP_Text>();
        fullText = tmpComponent.text;
        delay = 1f / charactersPerSecond;
    }

    void OnEnable()
    {
        if (playOnEnable && autoStart)
        {
            StartTypewriter();
        }
    }

    public void StartTypewriter()
    {
        StartCoroutine(TypeText());
    }

    public void SkipToEnd()
    {
        StopAllCoroutines();
        tmpComponent.text = fullText;
    }

    IEnumerator TypeText()
    {
        tmpComponent.text = ""; // Clear the text
        tmpComponent.maxVisibleCharacters = 0;
        tmpComponent.text = fullText;

        for (int i = 0; i <= fullText.Length; i++)
        {
            tmpComponent.maxVisibleCharacters = i;
            yield return new WaitForSeconds(delay);
        }
    }
}

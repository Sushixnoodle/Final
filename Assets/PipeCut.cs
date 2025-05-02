using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PipeCut : MonoBehaviour
{
    public AudioClip soundClip; // Assign the sound clip in the Inspector
    public TextMeshProUGUI textObject; // Assign the TextMeshProUGUI object in the Inspector
    public string nextSceneName; // Name of the next scene to load
    public float letterDelay = 0.1f; // Delay between each letter (adjust as needed)

    private AudioSource audioSource;

    void Start()
    {
        // Get or add an AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = soundClip;

        // Start the sequence
        StartCoroutine(SceneSequenceCoroutine());
    }

    IEnumerator SceneSequenceCoroutine()
    {
        // Step 1: Play the sound for 3 seconds
        audioSource.Play();
        yield return new WaitForSeconds(2f);
        audioSource.Stop();

        // Step 2: Show nothing for 2 seconds
        yield return new WaitForSeconds(1f);

        // Step 3: Animate the text letter by letter
        if (textObject != null)
        {
            textObject.gameObject.SetActive(true);
            yield return StartCoroutine(TypeText(textObject, letterDelay));
        }

        // Step 4: Wait 2.5 seconds after the text animation
        yield return new WaitForSeconds(2.5f);

        // Step 5: Load the next scene
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    IEnumerator TypeText(TextMeshProUGUI text, float delay)
    {
        // Store the full text and clear the text object
        string fullText = text.text;
        text.text = "";

        // Reveal the text letter by letter
        for (int i = 0; i < fullText.Length; i++)
        {
            text.text += fullText[i]; // Add the next character
            yield return new WaitForSeconds(delay); // Wait before adding the next character
        }
    }
}
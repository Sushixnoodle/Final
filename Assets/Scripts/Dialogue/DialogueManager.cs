using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public Button choiceButton1;
    public Button choiceButton2;
    public GameObject dialoguePanel;

    [Header("Text Animation Settings")]
    [Tooltip("Speed at which characters appear (lower = faster)")]
    public float textSpeed = 0.05f;
    [Tooltip("Should text animate?")]
    public bool animateText = true;

    private Dialogue currentDialogue;
    private int currentLineIndex;
    private bool isWaitingForClick;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    private GameObject voiceClipPlayer;

    public void StartDialogue(Dialogue dialogue)
    {
        if (dialogue == null)
        {
            Debug.LogError("Dialogue is null. Please assign a valid Dialogue asset.");
            return;
        }

        currentDialogue = dialogue;
        currentLineIndex = 0;

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Dialogue Panel is not assigned.");
        }

        isWaitingForClick = false;
        ShowDialogueLine();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // If text is still animating, complete it instantly
            if (isTyping)
            {
                CompleteTextAnimation();
                return;
            }

            // If we were waiting for a click and no choices are shown, skip to next line
            if (isWaitingForClick)
            {
                DialogueLine line = currentDialogue.dialogueLines[currentLineIndex];

                if (line.choices == null || line.choices.Length == 0)
                {
                    if (line.nextLineIndex >= 0 && line.nextLineIndex < currentDialogue.dialogueLines.Length)
                    {
                        currentLineIndex = line.nextLineIndex;
                        isWaitingForClick = false; // reset flag
                        ShowDialogueLine();
                    }
                    else
                    {
                        EndDialogue();
                    }
                }
            }
        }
    }


    private void ShowDialogueLine()
    {
        if (currentDialogue == null || currentDialogue.dialogueLines == null || currentDialogue.dialogueLines.Length == 0)
        {
            Debug.LogError("Dialogue or dialogue lines are not properly configured.");
            return;
        }

        DialogueLine line = currentDialogue.dialogueLines[currentLineIndex];

        if (speakerNameText != null)
        {
            speakerNameText.text = line.speakerName;
        }

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        if (animateText)
        {
            typingCoroutine = StartCoroutine(TypeText(line.dialogueText));
        }
        else
        {
            dialogueText.text = line.dialogueText;
            isTyping = false;
        }

        // Stop the previous clip (if it's still playing)
        if (voiceClipPlayer != null)
        {
            Destroy(voiceClipPlayer);
        }

        if (line.voiceClip != null)
        {
            voiceClipPlayer = new GameObject("TempVoicePlayer");
            var audioSource = voiceClipPlayer.AddComponent<AudioSource>();
            audioSource.clip = line.voiceClip;
            audioSource.Play();

            Destroy(voiceClipPlayer, line.voiceClip.length + 0.1f); // Clean up after playing
        }


        // Hide buttons by default
        if (choiceButton1 != null) choiceButton1.gameObject.SetActive(false);
        if (choiceButton2 != null) choiceButton2.gameObject.SetActive(false);

        // Show choices if available
        if (line.choices != null && line.choices.Length > 0)
        {
            SetupChoiceButton(choiceButton1, 0, line);
            SetupChoiceButton(choiceButton2, 1, line);
        }
        else
        {
            isWaitingForClick = true;
        }
    }

    private void SetupChoiceButton(Button button, int choiceIndex, DialogueLine line)
    {
        if (button != null && line.choices.Length > choiceIndex)
        {
            button.gameObject.SetActive(true);
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null) buttonText.text = line.choices[choiceIndex];

            button.onClick.RemoveAllListeners();
            int index = choiceIndex;
            button.onClick.AddListener(() => OnChoiceSelected(index));
        }
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    private void CompleteTextAnimation()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        DialogueLine line = currentDialogue.dialogueLines[currentLineIndex];
        dialogueText.text = line.dialogueText;
        isTyping = false;
    }

    private void OnChoiceSelected(int choiceIndex)
    {
        DialogueLine line = currentDialogue.dialogueLines[currentLineIndex];
        int nextLineIndex = line.nextLines[choiceIndex];

        if (nextLineIndex >= 0 && nextLineIndex < currentDialogue.dialogueLines.Length)
        {
            currentLineIndex = nextLineIndex;
            ShowDialogueLine();
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        GoToNextScene();
    }

    private void GoToNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}

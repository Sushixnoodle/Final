using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance; // Singleton instance

    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public Button choiceButton1; // Assign in Inspector
    public Button choiceButton2; // Assign in Inspector
    public GameObject dialoguePanel;

    [SerializeField] private float textSpeed = 0.05f; // Speed of text animation (adjust in Inspector)

    private Dialogue currentDialogue;
    private int currentLineIndex;
    private bool isWaitingForClick; // Track if we're waiting for a click to continue
    private bool isTextAnimating; // Track if text is currently animating

    private void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (dialogue == null)
        {
            Debug.LogError("Dialogue is null. Please assign a valid Dialogue asset.");
            return;
        }

        Debug.Log("Starting dialogue: " + dialogue.name);
        currentDialogue = dialogue;
        currentLineIndex = 0;

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true); // Enable the panel
        }
        else
        {
            Debug.LogError("Dialogue Panel is not assigned.");
        }

        isWaitingForClick = false; // Reset click state
        ShowDialogueLine();
    }

    private void Update()
    {
        // Check for mouse click (or touch) to continue dialogue
        if (isWaitingForClick && Input.GetMouseButtonDown(0)) // Left mouse button or touch
        {
            if (isTextAnimating)
            {
                // If text is still animating, skip to the end
                StopAllCoroutines();
                dialogueText.text = currentDialogue.dialogueLines[currentLineIndex].dialogueText;
                isTextAnimating = false;
                ShowChoices(); // Show choices after text animation is complete
            }
            else
            {
                DialogueLine line = currentDialogue.dialogueLines[currentLineIndex];

                if (line.choices == null || line.choices.Length == 0)
                {
                    // If there are no choices, use the nextLineIndex
                    if (line.nextLineIndex >= 0 && line.nextLineIndex < currentDialogue.dialogueLines.Length)
                    {
                        currentLineIndex = line.nextLineIndex; // Move to the specified next line
                        ShowDialogueLine();
                    }
                    else
                    {
                        // If nextLineIndex is invalid, end the dialogue
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
        else
        {
            Debug.LogError("Speaker Name Text is not assigned.");
        }

        if (dialogueText != null)
        {
            // Start text animation
            StartCoroutine(AnimateText(line.dialogueText));
        }
        else
        {
            Debug.LogError("Dialogue Text is not assigned.");
        }

        // Hide choice buttons by default
        if (choiceButton1 != null)
        {
            choiceButton1.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Choice Button 1 is not assigned.");
        }

        if (choiceButton2 != null)
        {
            choiceButton2.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Choice Button 2 is not assigned.");
        }

        // If there are no choices, wait for a click to continue
        if (line.choices == null || line.choices.Length == 0)
        {
            isWaitingForClick = true;
        }
    }

    private IEnumerator AnimateText(string fullText)
    {
        isTextAnimating = true;
        dialogueText.text = ""; // Clear the text

        // Animate the text letter by letter
        for (int i = 0; i < fullText.Length; i++)
        {
            dialogueText.text += fullText[i]; // Add one character
            yield return new WaitForSeconds(textSpeed); // Wait before adding the next character
        }

        isTextAnimating = false;

        // Show choices if they exist
        DialogueLine line = currentDialogue.dialogueLines[currentLineIndex];
        if (line.choices != null && line.choices.Length > 0)
        {
            ShowChoices();
        }
    }

    private void ShowChoices()
    {
        DialogueLine line = currentDialogue.dialogueLines[currentLineIndex];

        // Set up Choice 1
        if (line.choices.Length > 0 && choiceButton1 != null)
        {
            choiceButton1.gameObject.SetActive(true);
            TextMeshProUGUI choice1Text = choiceButton1.GetComponentInChildren<TextMeshProUGUI>();
            if (choice1Text != null)
            {
                choice1Text.text = line.choices[0];
            }
            else
            {
                Debug.LogError("Choice Button 1 Text is missing.");
            }
            choiceButton1.onClick.RemoveAllListeners(); // Clear previous listeners
            choiceButton1.onClick.AddListener(() => OnChoiceSelected(0)); // Trigger choice 1
        }

        // Set up Choice 2
        if (line.choices.Length > 1 && choiceButton2 != null)
        {
            choiceButton2.gameObject.SetActive(true);
            TextMeshProUGUI choice2Text = choiceButton2.GetComponentInChildren<TextMeshProUGUI>();
            if (choice2Text != null)
            {
                choice2Text.text = line.choices[1];
            }
            else
            {
                Debug.LogError("Choice Button 2 Text is missing.");
            }
            choiceButton2.onClick.RemoveAllListeners(); // Clear previous listeners
            choiceButton2.onClick.AddListener(() => OnChoiceSelected(1)); // Trigger choice 2
        }
    }

    private void OnChoiceSelected(int choiceIndex)
    {
        DialogueLine line = currentDialogue.dialogueLines[currentLineIndex];
        int nextLineIndex = line.nextLines[choiceIndex]; // Get the next line index for the chosen choice

        if (nextLineIndex >= 0 && nextLineIndex < currentDialogue.dialogueLines.Length)
        {
            currentLineIndex = nextLineIndex; // Move to the next line
            ShowDialogueLine();
        }
        else
        {
            EndDialogue(); // End dialogue if the next line is invalid
        }
    }

    private void EndDialogue()
    {
        Debug.Log("Dialogue ended.");
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false); // Disable the panel
        }

        // Transition to the next scene
        GoToNextScene();
    }

    private void GoToNextScene()
    {
        // Load the next scene
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No next scene available. This is the last scene.");
        }
    }
}
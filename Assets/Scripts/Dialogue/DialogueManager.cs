using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public Button choiceButton1; // Assign in Inspector
    public Button choiceButton2; // Assign in Inspector
    public GameObject dialoguePanel;

    private Dialogue currentDialogue;
    private int currentLineIndex;
    private bool isWaitingForClick; // Track if we're waiting for a click to continue

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
            if (currentLineIndex == currentDialogue.dialogueLines.Length - 1)
            {
                // If this is the last line, go to the next scene
                GoToNextScene();
            }
            else
            {
                // Otherwise, proceed to the next line
                NextLine();
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
            dialogueText.text = line.dialogueText;
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

        // Show and set up choice buttons if choices exist
        if (line.choices != null && line.choices.Length > 0)
        {
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
        else
        {
            // If there are no choices, wait for a click to continue
            isWaitingForClick = true;
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

    private void NextLine()
    {
        if (currentLineIndex < currentDialogue.dialogueLines.Length - 1)
        {
            currentLineIndex++; // Move to the next line
            ShowDialogueLine();
        }
        else
        {
            EndDialogue(); // End dialogue if there are no more lines
        }
    }

    private void EndDialogue()
    {
        Debug.Log("Dialogue ended.");
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false); // Disable the panel
        }
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
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject choicePanel;
    public Button choiceButtonPrefab;
    public GameObject dialoguePanel;

    private Dialogue currentDialogue;
    private int currentLineIndex;
    private bool isWaitingForClick; // Track if we're waiting for a click to continue

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true); // Enable the panel
        isWaitingForClick = false; // Reset click state
        ShowDialogueLine();
    }

    private void Update()
    {
        // Check for mouse click (or touch) to continue dialogue
        if (isWaitingForClick && Input.GetMouseButtonDown(0)) // Left mouse button or touch
        {
            NextLine();
        }
    }

    private void ShowDialogueLine()
    {
        DialogueLine line = currentDialogue.dialogueLines[currentLineIndex];
        speakerNameText.text = line.speakerName;
        dialogueText.text = line.dialogueText;

        // Clear existing choice buttons
        foreach (Transform child in choicePanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Add choice buttons if choices exist
        if (line.choices != null && line.choices.Length > 0)
        {
            for (int i = 0; i < line.choices.Length; i++)
            {
                // Instantiate a new button for each choice
                Button choiceButton = Instantiate(choiceButtonPrefab, choicePanel.transform);
                TextMeshProUGUI choiceText = choiceButton.GetComponentInChildren<TextMeshProUGUI>();
                choiceText.text = line.choices[i];
                int choiceIndex = i; // Capture the index for the listener
                choiceButton.onClick.AddListener(() => OnChoiceSelected(choiceIndex));
            }
        }
        else
        {
            isWaitingForClick = true; // Wait for click to continue
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
        dialoguePanel.SetActive(false); // Disable the panel
        isWaitingForClick = false; // Reset click state
    }
}
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text speakerNameText;
    public Text dialogueText;
    public GameObject choicePanel;
    public Button choiceButtonPrefab;

    private Dialogue currentDialogue;
    private int currentLineIndex;

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        currentLineIndex = 0;
        ShowDialogueLine();
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
                Button choiceButton = Instantiate(choiceButtonPrefab, choicePanel.transform);
                choiceButton.GetComponentInChildren<Text>().text = line.choices[i];
                int choiceIndex = i; // Capture the index for the listener
                choiceButton.onClick.AddListener(() => OnChoiceSelected(choiceIndex));
            }
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
        // Clean up or close the dialogue UI
    }
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

[System.Serializable]
public class DialogueLine
{
    public string characterName;
    [TextArea(2, 5)] public string dialogueText;
    public bool isCharacterOne; // True = Character 1, False = Character 2
}

public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialoguePanel; // Panel for dialogue UI
    public TextMeshProUGUI nameText; // Character name UI text
    public TextMeshProUGUI dialogueText; // Dialogue UI text

    [Header("Fonts")]
    public TMP_FontAsset characterOneFont; // Font for Character 1's name
    public TMP_FontAsset characterTwoFont; // Font for Character 2's name

    [Header("Font Sizes")]
    public int characterOneFontSize = 36; // Font size for Character 1's name
    public int characterTwoFontSize = 36; // Font size for Character 2's name

    [Header("Dialogue Settings")]
    public DialogueLine[] dialogueLines; // Array of dialogue lines
    public string nextSceneName; // Scene to load after dialogue
    public float textSpeed = 0.05f; // Speed of the typewriter effect

    private int currentLineIndex = 0;
    private bool isTyping = false;

    void Start()
    {
        dialoguePanel.SetActive(true);
        ShowNextLine();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Click to progress
        {
            if (isTyping)
            {
                // If the text is still typing, skip to the end
                StopAllCoroutines();
                dialogueText.text = dialogueLines[currentLineIndex - 1].dialogueText;
                isTyping = false;
            }
            else
            {
                ShowNextLine();
            }
        }
    }

    void ShowNextLine()
    {
        if (currentLineIndex < dialogueLines.Length)
        {
            DialogueLine line = dialogueLines[currentLineIndex];
            nameText.text = line.characterName;

            // Set font and font size for the character's name only
            if (line.isCharacterOne)
            {
                nameText.font = characterOneFont;
                nameText.fontSize = characterOneFontSize;
            }
            else
            {
                nameText.font = characterTwoFont;
                nameText.fontSize = characterTwoFontSize;
            }

            StartCoroutine(TypeText(line.dialogueText));
            currentLineIndex++;
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = ""; // Clear the text before starting the typewriter effect
        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed); // Wait before showing the next character
        }
        isTyping = false;
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false); // Hide the dialogue panel
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName); // Load the next scene
        }
    }
}
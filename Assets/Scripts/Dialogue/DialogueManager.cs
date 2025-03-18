using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

    private int currentLineIndex = 0;

    void Start()
    {
        dialoguePanel.SetActive(true);
        ShowNextLine();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Click to progress
        {
            ShowNextLine();
        }
    }

    void ShowNextLine()
    {
        if (currentLineIndex < dialogueLines.Length)
        {
            DialogueLine line = dialogueLines[currentLineIndex];
            nameText.text = line.characterName;
            dialogueText.text = line.dialogueText;

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

            currentLineIndex++;
        }
        else
        {
            EndDialogue();
        }
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
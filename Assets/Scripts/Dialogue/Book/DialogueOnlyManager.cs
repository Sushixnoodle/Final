using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueOnlyManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public Button choiceButton1;
    public Button choiceButton2;
    public GameObject dialoguePanel;

    [Header("Text Animation Settings")]
    public float textSpeed = 0.05f;
    public bool animateText = true;

    private Dialogue currentDialogue;
    private int currentLineIndex;
    private bool isWaitingForClick;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

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
        if (isWaitingForClick && Input.GetMouseButtonDown(0))
        {
            DialogueLine line = currentDialogue.dialogueLines[currentLineIndex];

            if (isTyping)
            {
                CompleteTextAnimation();
                return;
            }

            if (line.choices == null || line.choices.Length == 0)
            {
                if (line.nextLineIndex >= 0 && line.nextLineIndex < currentDialogue.dialogueLines.Length)
                {
                    currentLineIndex = line.nextLineIndex;
                    ShowDialogueLine();
                }
                else
                {
                    EndDialogue();
                }
            }
        }
    }

    private void ShowDialogueLine()
    {
        DialogueLine line = currentDialogue.dialogueLines[currentLineIndex];

        speakerNameText.text = line.speakerName;

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

        // Handle choices
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);

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
        if (line.choices.Length > choiceIndex)
        {
            button.gameObject.SetActive(true);
            button.GetComponentInChildren<TextMeshProUGUI>().text = line.choices[choiceIndex];
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnChoiceSelected(choiceIndex));
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
        StopCoroutine(typingCoroutine);
        dialogueText.text = currentDialogue.dialogueLines[currentLineIndex].dialogueText;
        isTyping = false;
    }

    private void OnChoiceSelected(int choiceIndex)
    {
        int nextLineIndex = currentDialogue.dialogueLines[currentLineIndex].nextLines[choiceIndex];

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
        dialoguePanel.SetActive(false);
    }
}
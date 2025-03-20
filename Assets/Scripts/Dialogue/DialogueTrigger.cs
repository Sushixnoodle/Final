using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue; // Assign your Dialogue asset in the Inspector

    void Awake()
    {
        DontDestroyOnLoad(dialogue);  // Keeps this object from being destroyed on scene load
    }
    private void Start()
    {
        // Trigger the dialogue automatically when the game starts
        TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue(dialogue);
        }
    }
}

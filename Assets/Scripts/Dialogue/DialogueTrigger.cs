using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue; // Assign your Dialogue asset in the Inspector
    public float interactionDistance = 3f; // Set the interaction distance

    private GameObject player; // Reference to the player

    private void Start()
    {
        // Find the player GameObject (ensure it has a "Player" tag)
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        // Check if the player is within interaction distance
        if (player != null && Vector3.Distance(transform.position, player.transform.position) <= interactionDistance)
        {
            // Check if the E key is pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                TriggerDialogue();
            }
        }
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
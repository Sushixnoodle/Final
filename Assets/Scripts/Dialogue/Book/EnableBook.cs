using UnityEngine;

public class EnableBook : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public Dialogue dialogue;
    public float interactionDistance = 3f;

    private DialogueOnlyManager dialogueManager;
    private Camera mainCamera;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueOnlyManager>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (CanInteract() && Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance) && hit.collider.gameObject == gameObject)
            {
                TriggerDialogue();
            }
        }
    }

    private bool CanInteract()
    {
        return dialogueManager != null &&
               dialogueManager.dialoguePanel != null &&
               !dialogueManager.dialoguePanel.activeSelf;
    }

    public void TriggerDialogue()
    {
        if (dialogueManager != null && dialogue != null)
        {
            dialogueManager.StartDialogue(dialogue);
        }
    }
}
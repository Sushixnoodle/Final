using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableBook : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public Dialogue dialogue;
    public float interactionDistance = 3f;

    private DialogueOnlyManager dialogueOnlyManager;
    private Camera mainCamera;

    private void Start()
    {
        dialogueOnlyManager = FindObjectOfType<DialogueOnlyManager>();
        if (dialogueOnlyManager == null)
        {
            Debug.LogError("No DialogueManager found in the scene!");
        }

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("No main camera found in the scene!");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    TriggerDialogue();
                }
            }
        }
    }

    public void TriggerDialogue()
    {
        if (dialogueOnlyManager != null && dialogue != null)
        {
            dialogueOnlyManager.StartDialogue(dialogue);
        }
    }
}
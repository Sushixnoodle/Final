using TMPro;
using UnityEngine;

public class DistanceChecker : MonoBehaviour
{
    public TextMeshProUGUI uiText; // Assign the TextMeshPro UI Text in the inspector
    public float interactionDistance = 3f; // Distance within which interaction is possible
    private bool isTextActive = false;

    void Update()
    {
        CheckDistance();
    }

    void CheckDistance()
    {
        // Check distance between player and this object
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        if (distance <= interactionDistance && !isTextActive)
        {
            ShowText();
        }
        else if (distance > interactionDistance && isTextActive)
        {
            HideText();
        }
    }

    void ShowText()
    {
        isTextActive = true;
        uiText.gameObject.SetActive(true);
    }

    void HideText()
    {
        isTextActive = false;
        uiText.gameObject.SetActive(false);
    }
}
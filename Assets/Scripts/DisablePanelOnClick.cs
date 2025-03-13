using UnityEngine;

public class DisablePanelOnClick : MonoBehaviour
{
    public GameObject panelToDisable; // Reference to the panel to disable

    // This method is called when the button is clicked
    public void OnButtonClick()
    {
        if (panelToDisable != null)
        {
            panelToDisable.SetActive(false); // Disable the panel
        }
        else
        {
            Debug.LogWarning("Panel to disable is not assigned!");
        }
    }
}
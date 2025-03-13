using UnityEngine;

public class EnablePanelOnClick : MonoBehaviour
{
    public GameObject panelToEnable; // Reference to the panel to enable

    // This method is called when the button is clicked
    public void OnButtonClick()
    {
        if (panelToEnable != null)
        {
            panelToEnable.SetActive(true); // Enable the panel
        }
        else
        {
            Debug.LogWarning("Panel to enable is not assigned!");
        }
    }
}
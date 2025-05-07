using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelToggleController : MonoBehaviour
{
    public GameObject panel;     // The panel to open/close
    public Button openButton;    // Button to open the panel

    private bool isPanelOpen = false;

    void Start()
    {
        panel.SetActive(false); // Make sure panel starts closed
        openButton.onClick.AddListener(OpenPanel);
    }

    void Update()
    {
        if (isPanelOpen && Input.GetMouseButtonDown(0))
        {
            // Check if the click was not on a UI element (e.g. not the panel itself)
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                ClosePanel();
            }
        }
    }

    void OpenPanel()
    {
        panel.SetActive(true);
        isPanelOpen = true;
    }

    void ClosePanel()
    {
        panel.SetActive(false);
        isPanelOpen = false;
    }
}



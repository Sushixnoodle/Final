using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelToggleController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject panel;         // The panel to open/close
    public Button openButton;        // Button to open the panel
    public Button closeButton;       // Button to close the panel (drag this in inspector)

    private bool isPanelOpen = false;

    void Start()
    {
        // Initialize panel state
        panel.SetActive(false);

        // Set up button listeners
        openButton.onClick.AddListener(OpenPanel);
        closeButton.onClick.AddListener(ClosePanel);
    }

    void Update()
    {
        // Close panel when clicking outside (optional)
        if (isPanelOpen && Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIElement())
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

    public void ClosePanel() // Made public so button can call it
    {
        panel.SetActive(false);
        isPanelOpen = false;
    }

    // Helper method to check if pointer is over any UI element
    private bool IsPointerOverUIElement()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }
}
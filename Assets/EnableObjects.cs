using UnityEngine;

public class EnableObjects : MonoBehaviour
{
    public GameObject objectA; // Reference to GameObject A
    public GameObject objectB; // Reference to GameObject B

    private void OnEnable()
    {
        // When this script's GameObject is enabled
        if (objectA != null && objectB != null)
        {
            // Enable objectB when objectA is enabled
            objectB.SetActive(objectA.activeSelf);
        }
    }

    private void Update()
    {
        // Continuously check if objectA is enabled/disabled (optional)
        if (objectA != null && objectB != null)
        {
            if (objectB.activeSelf != objectA.activeSelf)
            {
                objectB.SetActive(objectA.activeSelf);
            }
        }
    }
}
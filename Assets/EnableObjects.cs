using UnityEngine;

public class EnableDisableObjects : MonoBehaviour
{
    public GameObject objectA; // Reference to GameObject A
    public GameObject objectB; // Will be enabled when A is enabled
    public GameObject objectC; // Will be disabled when A is enabled

    private void OnEnable()
    {
        if (objectA != null && objectA.activeSelf)
        {
            if (objectB != null) objectB.SetActive(true);
            if (objectC != null) objectC.SetActive(false);
        }
    }

    private void Update()
    {
        // Optional: Continuously sync if A's state changes at runtime
        if (objectA != null && objectA.activeSelf)
        {
            if (objectB != null && !objectB.activeSelf) objectB.SetActive(true);
            if (objectC != null && objectC.activeSelf) objectC.SetActive(false);
        }
    }
}
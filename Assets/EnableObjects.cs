using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [Header("References")]
    public GameObject objectA;  // The trigger object (optional if script is on A)
    public GameObject objectB;  // Enable this when A is enabled
    public GameObject objectC;  // Disable this when A is enabled
    public GameObject objectD;  // Destroy this when A is enabled
    public GameObject objectE;  // Also destroy this when A is enabled

    private void Start()
    {
        // If objectA is not assigned, assume this script is on A
        if (objectA == null) objectA = gameObject;
    }

    private void OnEnable()
    {
        if (objectB != null) objectB.SetActive(true);   // Enable B
        if (objectC != null) objectC.SetActive(false);  // Disable C
        if (objectD != null) Destroy(objectD);          // Destroy D
        if (objectE != null) Destroy(objectE);          // Destroy E
    }

    // Optional: Reset behavior if A is disabled
    private void OnDisable()
    {
        if (objectB != null) objectB.SetActive(false);
        if (objectC != null) objectC.SetActive(true);
        // Note: D and E are permanently destroyed and cannot be restored
    }
}
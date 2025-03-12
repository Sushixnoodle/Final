using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    
    public Transform target;

    
    public float distance = 10f;

    
    public float orbitSpeed = 50f;

    void Update()
    {
        // Ensure there's a target assigned
        if (target == null)
        {
            Debug.LogWarning("No target assigned for the camera to orbit around.");
            return;
        }

        // Rotate the camera around the target
        transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime);

        // Update the camera's position to maintain the set distance from the target
        transform.position = target.position - transform.forward * distance;

        // Make the camera look at the target
        transform.LookAt(target);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KingsTrigger : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float activationDistance = 3f; // Distance within which the player can interact
    public string sceneName; // Name of the scene to load

    void Update()
    {
        // Check if the player is within the activation distance
        if (Vector3.Distance(player.position, transform.position) <= activationDistance)
        {
            // Check if the "E" key is pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Load the specified scene
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    // Optional: Draw a gizmo in the editor to visualize the activation distance
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activationDistance);
    }
}
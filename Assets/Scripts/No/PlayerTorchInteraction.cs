using UnityEngine;

public class PlayerTorchInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask torchLayer; // Set this to the Torch layer in Inspector
    [SerializeField] private float interactionDistance = 3f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Press E to light/unlight a torch
        {
            TryLightTorch();
        }
    }

    private void TryLightTorch()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance, torchLayer))
        {
            Torch torch = hit.collider.GetComponent<Torch>();
            if (torch != null)
            {
                torch.ToggleTorch();
            }
        }
    }
}

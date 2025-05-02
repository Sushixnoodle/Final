using UnityEngine;

public class DisableOnPlayerTrigger : MonoBehaviour
{
    [Tooltip("Assign the trigger zone here (must have 'Is Trigger' checked)")]
    public Collider triggerZone;

    [Tooltip("Tag to identify the player")]
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (triggerZone == null) return;

        if (other.CompareTag(playerTag))
        {
            gameObject.SetActive(false);
        }
    }
}

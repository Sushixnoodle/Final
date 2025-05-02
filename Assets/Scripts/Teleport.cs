using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform target;
    public float teleportCooldown = 1f; // Cooldown duration in seconds

    private static bool isTeleporting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isTeleporting) return;

        CharacterController controller = other.gameObject.GetComponent<CharacterController>();
        if (controller != null)
        {
            StartCoroutine(TeleportWithCooldown(other.gameObject, controller));
        }
    }

    private IEnumerator TeleportWithCooldown(GameObject player, CharacterController controller)
    {
        isTeleporting = true;
        controller.enabled = false;
        player.transform.position = target.position;
        controller.enabled = true;

        yield return new WaitForSeconds(teleportCooldown);
        isTeleporting = false;
    }
}

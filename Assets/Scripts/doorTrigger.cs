using UnityEngine;

public class doorTrigger : MonoBehaviour
{
    public Animator animator;
    private bool isPlayerNearby = false;
    private bool isOpen = false;

    void Update()
    {
        if (isPlayerNearby && Input.GetMouseButtonDown(0)) // 0 = Left Mouse Button
        {
            isOpen = !isOpen;
            animator.SetBool("isOpen", isOpen);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}

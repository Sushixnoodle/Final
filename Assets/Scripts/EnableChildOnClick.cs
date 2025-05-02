using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnableChildOnClick : MonoBehaviour
{
    public GameObject childObject; // Assign the child GameObject in the Inspector
    public Image uiImage;          // Assign the UI Image in the Inspector
    public string nextSceneName;   // Name of the next scene to load
    public float maxDistance = 5f; // Maximum distance for interaction

    private float holdTimer = 0f;
    private bool isHolding = false;
    private Animator animator;

    private void Start()
    {
        if (childObject != null)
        {
            animator = childObject.GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) // Left mouse button
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if it's the correct object and within distance
                if (hit.collider.gameObject == gameObject &&
                    Vector3.Distance(Camera.main.transform.position, hit.point) <= maxDistance)
                {
                    if (!isHolding)
                    {
                        if (animator != null)
                        {
                            animator.Play("USING THISZ", -1, 0f);
                        }

                        if (childObject != null)
                        {
                            childObject.SetActive(true);
                        }

                        if (uiImage != null)
                        {
                            uiImage.enabled = true;
                        }

                        isHolding = true;
                    }

                    holdTimer += Time.deltaTime;

                    if (holdTimer >= 5f)
                    {
                        if (!string.IsNullOrEmpty(nextSceneName))
                        {
                            SceneManager.LoadScene(nextSceneName);
                        }
                    }
                }
                else
                {
                    ResetTimer();
                }
            }
            else
            {
                ResetTimer();
            }
        }
        else
        {
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        isHolding = false;
        holdTimer = 0f;

        if (childObject != null)
        {
            childObject.SetActive(false);
        }

        if (uiImage != null)
        {
            uiImage.enabled = false;
        }
    }

    // Optional: Visualize the max distance in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
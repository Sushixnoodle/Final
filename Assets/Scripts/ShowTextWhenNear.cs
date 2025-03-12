using UnityEngine;
using TMPro; 

public class ShowTextWhenNear : MonoBehaviour
{
    public Transform player; 
    public float showTextDistance = 3f; 
    public TextMeshPro textMeshPro; 

    void Start()
    {
        // Ensure the text is hidden at the start
        if (textMeshPro != null)
        {
            textMeshPro.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("TextMeshPro component is not assigned!");
        }
    }

    void Update()
    {
        if (player == null || textMeshPro == null)
        {
            Debug.LogWarning("Player or TextMeshPro reference is missing!");
            return;
        }

        // Calculate the distance between the player and this object
        float distance = Vector3.Distance(player.position, transform.position);

        // Show or hide the text based on the distance
        if (distance <= showTextDistance)
        {
            textMeshPro.gameObject.SetActive(true); 
        }
        else
        {
            textMeshPro.gameObject.SetActive(false); 
        }
    }
}
using UnityEngine;

public class PipePuzzleManager : MonoBehaviour
{
    public Pipe[] pipes;
    public GameObject targetObject;

    void Update()
    {
        bool allCorrect = true;
        foreach (Pipe pipe in pipes)
        {
            if (!pipe.IsCorrect())
            {
                allCorrect = false;
                break;
            }
        }

        if (allCorrect)
        {
            targetObject.SetActive(true); // Enable the object
        }
        else
        {
            targetObject.SetActive(false); // Disable the object
        }
    }
}
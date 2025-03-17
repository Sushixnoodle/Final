using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    private Dialogue selectedDialogue;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadDialogueScene(Dialogue dialogue)
    {
        selectedDialogue = dialogue; // Store the dialogue asset
        SceneManager.LoadScene("LordsCut"); // Load the dialogue scene
    }

    public Dialogue GetSelectedDialogue()
    {
        return selectedDialogue;
    }
}
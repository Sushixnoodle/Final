using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAnimationOnce : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();

        // Play the animation
        PlayAnimation("USING THISZ");
    }

    void PlayAnimation(string stateName)
    {
        // Set the animation to play once
        animator.Play(stateName, -1, 0f);
    }

    // This method will be called by the Animation Event
    public void OnAnimationEnd()
    {
        // Load the next scene
        SceneManager.LoadScene("PipeCut");
    }
}
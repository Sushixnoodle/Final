using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraAnimationSceneLoader : MonoBehaviour
{
    public Animator cameraAnimator;
    public string nextSceneName;

    private bool hasLoadedScene = false;
    private float clipLength;

    void Start()
    {
        if (cameraAnimator)
        {
            // Assumes only one state and one clip in the base layer
            AnimationClip[] clips = cameraAnimator.runtimeAnimatorController.animationClips;
            if (clips.Length > 0)
            {
                clipLength = clips[0].length;
            }
        }
    }

    void Update()
    {
        if (cameraAnimator && !hasLoadedScene)
        {
            AnimatorStateInfo animState = cameraAnimator.GetCurrentAnimatorStateInfo(0);
            float currentTime = animState.normalizedTime * clipLength;

            if (currentTime >= clipLength - 0.1f && !cameraAnimator.IsInTransition(0))
            {
                hasLoadedScene = true;
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}

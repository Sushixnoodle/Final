using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraAnimationSceneLoader : MonoBehaviour
{
    public Animator cameraAnimator;
    public string nextSceneName;

    private bool hasLoadedScene = false;

    void Update()
    {
        if (cameraAnimator && !hasLoadedScene)
        {
            AnimatorStateInfo animState = cameraAnimator.GetCurrentAnimatorStateInfo(0);

            if (animState.normalizedTime >= 1f && !cameraAnimator.IsInTransition(0))
            {
                hasLoadedScene = true;
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}

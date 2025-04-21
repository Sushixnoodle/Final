using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaySystem : MonoBehaviour
{
    [Header("Settings")]
    public int maxBounces = 10;
    public float maxDistance = 100f;
    public LayerMask reflectionLayers = ~0; // Everything (temporarily)
    public float rayOffset = 0.01f;

    [Header("References")]
    public LineRenderer line;

    [Header("Detection System")]
    public string nextSceneName; // Name of the scene to load
    public Color hitColor = Color.green; // Visual feedback when hit
    public float transitionDelay = 0.5f; // Delay before scene transition

    private Color originalColor;
    private bool isHit = false;

    void Start()
    {
        if (line == null) line = GetComponent<LineRenderer>();
        line.useWorldSpace = true; // Critical!
        line.positionCount = maxBounces + 1;
        line.material.color = Color.white;
    }

    void Update()
    {
        UpdateLaser();
    }

    void UpdateLaser()
    {
        // Reset to emitter position first
        for (int i = 0; i < line.positionCount; i++)
        {
            line.SetPosition(i, transform.position);
        }

        Vector3 currentPos = transform.position;
        Vector3 currentDir = transform.forward;
        line.SetPosition(0, currentPos);

        for (int i = 0; i < maxBounces; i++)
        {
            if (Physics.Raycast(currentPos, currentDir, out RaycastHit hit, maxDistance, reflectionLayers))
            {
                line.SetPosition(i + 1, hit.point);
                Debug.DrawLine(currentPos, hit.point, Color.green, 0.1f);
                StartCoroutine(OnLaserHit(hit));
                Debug.Log("Running UpdateLaser");

                // Offset to avoid self-collision
                currentPos = hit.point + (hit.normal * rayOffset);
                currentDir = Vector3.Reflect(currentDir, hit.normal);
            }
            else
            {
                line.SetPosition(i + 1, currentPos + currentDir * maxDistance);
                Debug.DrawRay(currentPos, currentDir * maxDistance, Color.red, 0.1f);
                break;
            }
        }
    }


    // This on laser hit system uses RaycastHit from UpdateLaser WHICH WILL ONLY RUN if
    // the layer is set to mirror this will then detect if it is hitting something called target
    // if so it will execute everything inside the if statement else it will not also 
    // it will try to get the material component in order to SWITCH colours if it cant then it'll log a warning and not change its colour.
    IEnumerator OnLaserHit(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Target"))
        {
            if(hit.collider.TryGetComponent<Renderer>(out Renderer targetmaterial))
            {
                targetmaterial.material.color = hitColor;
            }
            else
            {
                Debug.LogWarning("Target does not have renderer component");
            }
            Debug.Log("Running OnLaserHitFunction2");
            isHit = true;

            // play a sound here if you do have it but below is our timeout

            yield return new WaitForSeconds(5);

            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        Debug.Log("PREPARING TELELPORT!!!!!");
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Next scene name not specified on LaserTarget");
        }
    }
}

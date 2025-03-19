using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FireEnable : MonoBehaviour
{
    public GameObject targetObject; // The object to move in front of the player
    public float distance = 2.0f; // Distance in front of the player
    public GameObject panel; // The panel to enable when holding left click
    public AudioClip soundClip; // The sound to play when holding left click
    public string nextSceneName; // The name of the next scene to load

    private float clickHoldTime = 0f;
    private bool isHoldingClick = false;
    private AudioSource audioSource;

    void Start()
    {
        if (panel != null)
        {
            panel.SetActive(false); // Ensure the panel is disabled at start
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = soundClip;
    }

    void Update()
    {
        HandleLeftClick();
        HandleClickHold();
    }

    void HandleLeftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject) // Check if the clicked object is this one
                {
                    MoveObjectInFrontOfPlayer();
                }
            }
        }
    }

    void HandleClickHold()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject) // Check if the held object is this one
                {
                    isHoldingClick = true;
                    clickHoldTime += Time.deltaTime;

                    if (clickHoldTime >= 5f)
                    {
                        EnablePanelAndPlaySound();
                        LoadNextSceneAfterDelay(6f);
                        Debug.Log("Not holding");
                    }
                }
            }
        }
        else
        {
            isHoldingClick = false;
            clickHoldTime = 0f;
        }
    }

    void MoveObjectInFrontOfPlayer()
    {
        if (targetObject != null)
        {
            Vector3 playerPosition = Camera.main.transform.position;
            Vector3 playerForward = Camera.main.transform.forward;

            targetObject.transform.position = playerPosition + playerForward * distance;
            targetObject.transform.rotation = Quaternion.LookRotation(playerForward);
        }
    }

    void EnablePanelAndPlaySound()
    {
        if (panel != null)
        {
            panel.SetActive(true);
        }

        if (audioSource != null && soundClip != null)
        {
            audioSource.Play();
        }
    }

    void LoadNextSceneAfterDelay(float delay)
    {
        Invoke("LoadNextScene", delay);
    }

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}


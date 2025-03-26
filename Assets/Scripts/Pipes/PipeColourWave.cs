using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeColourWave : MonoBehaviour
{
    [System.Serializable]
    public class PipeData
    {
        public Renderer pipeRenderer;
        [HideInInspector] public Material originalMaterial;
        [HideInInspector] public Material runtimeMaterial;
        [HideInInspector] public Color originalColor;
    }

    [Header("Color Settings")]
    public Color targetColor = Color.blue;
    public float colorChangeDuration = 0.5f;
    public float delayBetweenPipes = 0.2f;

    [Header("Material Settings")]
    public string colorProperty = "_Color";
    public bool affectEmission = false;
    public string emissionProperty = "_EmissionColor";

    [Header("Pipe Configuration")]
    public List<PipeData> pipes = new List<PipeData>();
    public bool autoFindPipes = false;
    public string pipeTag = "Pipe";

    void Start()
    {
        InitializePipes();
        StartCoroutine(RunColorWave());
    }

    void InitializePipes()
    {
        if (autoFindPipes && pipes.Count == 0)
        {
            FindAndSortPipes();
        }

        foreach (PipeData pipe in pipes)
        {
            if (pipe.pipeRenderer != null)
            {
                pipe.originalMaterial = pipe.pipeRenderer.sharedMaterial;
                pipe.runtimeMaterial = new Material(pipe.originalMaterial);
                pipe.pipeRenderer.material = pipe.runtimeMaterial;

                pipe.originalColor = affectEmission ?
                    pipe.runtimeMaterial.GetColor(emissionProperty) :
                    pipe.runtimeMaterial.GetColor(colorProperty);
            }
        }
    }

    void FindAndSortPipes()
    {
        GameObject[] pipeObjects = GameObject.FindGameObjectsWithTag(pipeTag);
        System.Array.Sort(pipeObjects, (a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

        pipes.Clear();
        foreach (GameObject pipeObj in pipeObjects)
        {
            Renderer r = pipeObj.GetComponent<Renderer>();
            if (r != null)
            {
                pipes.Add(new PipeData { pipeRenderer = r });
            }
        }
    }

    IEnumerator RunColorWave()
    {
        foreach (PipeData pipe in pipes)
        {
            if (pipe.pipeRenderer != null)
            {
                StartCoroutine(ChangePipeColor(pipe));
                yield return new WaitForSeconds(delayBetweenPipes);
            }
        }
    }

    IEnumerator ChangePipeColor(PipeData pipe)
    {
        float elapsed = 0f;
        Material mat = pipe.runtimeMaterial;

        while (elapsed < colorChangeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / colorChangeDuration);
            Color newColor = Color.Lerp(pipe.originalColor, targetColor, t);

            if (affectEmission)
            {
                mat.SetColor(emissionProperty, newColor);
                mat.EnableKeyword("_EMISSION");
            }
            else
            {
                mat.SetColor(colorProperty, newColor);
            }

            yield return null;
        }

        if (affectEmission)
        {
            mat.SetColor(emissionProperty, targetColor);
        }
        else
        {
            mat.SetColor(colorProperty, targetColor);
        }
    }

    void OnDestroy()
    {
        foreach (PipeData pipe in pipes)
        {
            if (pipe.runtimeMaterial != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(pipe.runtimeMaterial);
                }
                else
                {
                    DestroyImmediate(pipe.runtimeMaterial);
                }
            }
        }
    }
}
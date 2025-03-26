using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeColourWave : MonoBehaviour
{
    public enum ColorDirection
    {
        LeftToRight,
        RightToLeft,
        UpToDown,
        DownToUp
    }

    [System.Serializable]
    public class PipeEntry
    {
        public Renderer pipeRenderer;
        public ColorDirection direction;
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
    public List<PipeEntry> pipes = new List<PipeEntry>();

    void Start()
    {
        InitializeMaterials();
        StartCoroutine(ExecuteColorWave());
    }

    void InitializeMaterials()
    {
        foreach (PipeEntry pipe in pipes)
        {
            if (pipe.pipeRenderer != null)
            {
                pipe.runtimeMaterial = new Material(pipe.pipeRenderer.sharedMaterial);
                pipe.pipeRenderer.material = pipe.runtimeMaterial;
                pipe.originalColor = affectEmission ?
                    pipe.runtimeMaterial.GetColor(emissionProperty) :
                    pipe.runtimeMaterial.GetColor(colorProperty);
            }
        }
    }

    IEnumerator ExecuteColorWave()
    {
        foreach (PipeEntry pipe in pipes)
        {
            if (pipe.pipeRenderer != null)
            {
                StartCoroutine(AnimatePipeColor(pipe));
                yield return new WaitForSeconds(delayBetweenPipes);
            }
        }
    }

    IEnumerator AnimatePipeColor(PipeEntry pipe)
    {
        float elapsed = 0f;
        Material mat = pipe.runtimeMaterial;
        Vector3 startPos = pipe.pipeRenderer.transform.position;
        float width = pipe.pipeRenderer.bounds.size.x;
        float height = pipe.pipeRenderer.bounds.size.y;

        while (elapsed < colorChangeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / colorChangeDuration);

            // Calculate progress based on direction
            float progress = 0f;
            switch (pipe.direction)
            {
                case ColorDirection.LeftToRight:
                    progress = Mathf.Lerp(startPos.x - width / 2, startPos.x + width / 2, t);
                    break;
                case ColorDirection.RightToLeft:
                    progress = Mathf.Lerp(startPos.x + width / 2, startPos.x - width / 2, t);
                    break;
                case ColorDirection.UpToDown:
                    progress = Mathf.Lerp(startPos.y + height / 2, startPos.y - height / 2, t);
                    break;
                case ColorDirection.DownToUp:
                    progress = Mathf.Lerp(startPos.y - height / 2, startPos.y + height / 2, t);
                    break;
            }

            // Apply color change
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

        // Ensure final color
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
        foreach (PipeEntry pipe in pipes)
        {
            if (pipe.runtimeMaterial != null)
            {
                Destroy(pipe.runtimeMaterial);
            }
        }
    }
}
using System.Collections;
using UnityEngine;

public class MaterialAnimator : MonoBehaviour
{
    public Renderer[] objects; // Assign objects left to right
    public Material targetMaterial; // The material to apply
    public float delayBetween = 0.5f; // Time between each material change

    void Start()
    {
        StartCoroutine(AnimateMaterials());
    }

    IEnumerator AnimateMaterials()
    {
        foreach (Renderer obj in objects)
        {
            obj.material = targetMaterial;
            yield return new WaitForSeconds(delayBetween);
        }
    }
}

using System.Collections;
using UnityEngine;

public class MaterialAnimator : MonoBehaviour
{
    public Renderer[] objects; // Assign left-to-right
    public float delayBetween = 0.5f;
    public float wipeDuration = 1f;

    void Start()
    {
        StartCoroutine(WipeInMaterials());
    }

    IEnumerator WipeInMaterials()
    {
        foreach (Renderer obj in objects)
        {
            Material mat = obj.material;

            // Make a unique instance of the material
            obj.material = new Material(mat);

            // Reset offset
            obj.material.mainTextureScale = new Vector2(1, 1);
            obj.material.mainTextureOffset = new Vector2(-1, 0);

            float timer = 0f;
            while (timer < wipeDuration)
            {
                float progress = timer / wipeDuration;
                obj.material.mainTextureOffset = new Vector2(-1 + progress, 0);
                obj.material.mainTextureScale = new Vector2(progress, 1);
                timer += Time.deltaTime;
                yield return null;
            }

            obj.material.mainTextureOffset = Vector2.zero;
            obj.material.mainTextureScale = Vector2.one;

            yield return new WaitForSeconds(delayBetween);
        }
    }
}

using UnityEngine;

public class LightPoint : MonoBehaviour
{
    public float lifeTime = 0.2f;
    private float currentLife;

    void OnEnable()
    {
        currentLife = lifeTime;
    }

    void Update()
    {
        currentLife -= Time.deltaTime;
        if (currentLife <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
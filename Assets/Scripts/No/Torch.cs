using UnityEngine;

public class Torch : MonoBehaviour
{
    public bool IsLit { get; private set; }
    private Renderer rend;
    private Material originalMaterial;

    [SerializeField] private Material whiteFireMaterial; // Assign white fire material for lit state
    [SerializeField] private Material redFireMaterial; // Assign red fire material for correct solution
    [SerializeField] private GameObject fireEffect; // Assign fire particle system object

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalMaterial = rend.material;
        fireEffect.SetActive(false); // Hide fire initially
    }

    public void ToggleTorch()
    {
        IsLit = !IsLit;
        fireEffect.SetActive(IsLit); // Show/hide fire effect
        if (IsLit)
        {
            rend.material = whiteFireMaterial; // White fire when first lit
        }
        else
        {
            rend.material = originalMaterial; // Revert to grey when unlit
        }
    }

    public void SetRedFire()
    {
        if (IsLit)
        {
            rend.material = redFireMaterial; // Set red fire when puzzle is solved
        }
    }
}

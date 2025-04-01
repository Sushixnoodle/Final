using UnityEngine;

public class TargetDetector : MonoBehaviour
{
    public ParticleSystem successParticles;
    public string requiredColor = "red";
    public bool isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        LineRenderer laser = other.GetComponent<LineRenderer>();
        if (laser != null && laser.startColor == GetColorFromString(requiredColor))
        {
            ActivateTarget();
        }
    }

    private void ActivateTarget()
    {
        isActivated = true;
        if (successParticles != null)
        {
            successParticles.Play();
        }
        // Add your game logic here (e.g., open door, play sound)
    }

    private Color GetColorFromString(string colorName)
    {
        switch (colorName.ToLower())
        {
            case "red": return Color.red;
            case "blue": return Color.blue;
            case "green": return Color.green;
            case "yellow": return Color.yellow;
            default: return Color.white;
        }
    }
}
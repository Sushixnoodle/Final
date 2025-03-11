using System.Collections.Generic;
using UnityEngine;

public class TorchPuzzle : MonoBehaviour
{
    [SerializeField] private List<Torch> torches; // Assign all torches
    [SerializeField] private List<Torch> correctTorches; // Assign the 3 correct torches
    private bool puzzleSolved = false;

    void Update()
    {
        if (!puzzleSolved && CheckPuzzleSolved())
        {
            ShowRedFire();
            puzzleSolved = true;
        }
    }

    private bool CheckPuzzleSolved()
    {
        int correctCount = 0;
        foreach (Torch torch in torches)
        {
            if (correctTorches.Contains(torch) && torch.IsLit)
            {
                correctCount++;
            }
            else if (!correctTorches.Contains(torch) && torch.IsLit)
            {
                return false; // A wrong torch is lit
            }
        }
        return correctCount == correctTorches.Count;
    }

    private void ShowRedFire()
    {
        foreach (Torch torch in torches)
        {
            torch.SetRedFire();
        }
    }
}

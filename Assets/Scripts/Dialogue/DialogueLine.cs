using System;
using UnityEngine;

[Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea] public string dialogueText;
    public string[] choices; // Array of choices
    public int[] nextLines; // Array of next line indices for each choice
    public int nextLineIndex = -1; // Default next line index
    public AudioClip voiceClip;

    public bool useCustomColor = false; // Toggle for using a custom color
    public Color customColor = Color.white; // Default to white (can be set in Inspector)
}

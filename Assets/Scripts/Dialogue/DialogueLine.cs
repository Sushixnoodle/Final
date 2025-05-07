using System;
using UnityEngine;

[Serializable]
public class DialogueLine
{
    public string speakerName;
    public string dialogueText;
    public string[] choices; // Array of choices
    public int[] nextLines; // Array of next line indices for each choice
    public int nextLineIndex = -1; // Default next line index (use -1 to indicate no next line)
    public AudioClip voiceClip; 
}
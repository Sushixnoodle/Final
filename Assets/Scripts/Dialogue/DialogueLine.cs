using System;
using UnityEngine;

[Serializable]
public class DialogueLine
{
    public string speakerName;
    public string dialogueText;
    public string[] choices; // Array of choices
    public int[] nextLines; // Array of next line indices for each choice
}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public DialogueLine[] dialogueLines;
}
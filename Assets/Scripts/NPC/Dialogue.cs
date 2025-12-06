using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string npcName;
    public Sprite portrait;
    [TextArea(3, 10)]
    public string[] sentences;
}
using UnityEngine;
// https://www.youtube.com/watch?v=jEoobucfoL4
[System.Serializable]
public struct SoundEffect
{
	public string groupID;
	public AudioClip[] clips;
	[Range(0f, 1f)] public float volume; // ← per-sound volume
}

public class SoundLibrary : MonoBehaviour
{
	public SoundEffect[] soundEffects;
	
	public AudioClip GetClipFromName(string name)
	{
		foreach (var soundEffect in soundEffects)
		{
			if (soundEffect.groupID == name)
			{
				return soundEffect.clips[Random.Range(0, soundEffect.clips.Length)];
			}
		}
		return null;
	}
}
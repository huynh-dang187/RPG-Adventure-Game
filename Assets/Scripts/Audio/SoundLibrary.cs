using UnityEngine;
// https://www.youtube.com/watch?v=jEoobucfoL4
[System.Serializable]
public struct SoundEffect
{
	public string groupID;
	public AudioClip[] clips;
	[Range(0f, 1f)] public float volume; // ← per-sound volume

    [HideInInspector] public int lastIndex; // nhớ clip trước đó
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

    // New: Get clip and volume together. If more sounds play random
    public bool TryGetClipAndVolume(string name, out AudioClip clip, out float volume)
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            var sfx = soundEffects[i];
            if (sfx.groupID == name)
            {
                // tránh lặp lại clip vừa phát
                int index;
                do
                {
                    index = Random.Range(0, sfx.clips.Length);
                } while (sfx.clips.Length > 1 && index == sfx.lastIndex);

                soundEffects[i].lastIndex = index; // lưu lại
                clip = sfx.clips[index];
                volume = sfx.volume;
                return true;
            }
        }
        clip = null;
        volume = 1f;
        return false;
    }
}
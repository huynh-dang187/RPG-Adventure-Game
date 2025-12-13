using UnityEngine;

// Source https://www.youtube.com/watch?v=jEoobucfoL4
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Library & Sources")]
    [SerializeField] 
    private SoundLibrary sfxLibrary;
    [SerializeField] 
    private AudioSource sfx2DSource;

    // --- THÊM MỚI: Nguồn phát nhạc nền ---
    [Header("Music")]
    public AudioSource musicSource; 
    // -------------------------------------

    private void Awake(){
        if (Instance!= null){
            Destroy(gameObject);
        }
        else{
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // --- THÊM MỚI: Hàm tắt nhạc để EndGame gọi ---
    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
    // ----------------------------------------------

    public void PlaySound3D(AudioClip clip, float volume, Vector3 pos)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos, volume);
        }
    }

    public void PlaySound3D(string soundName, Vector3 pos)
    {
        if (sfxLibrary.TryGetClipAndVolume(soundName, out var clip, out var volume))
        {
            PlaySound3D(clip, volume, pos);
        }
    }

    public void PlaySound2D(string soundName)
    {
        if (sfxLibrary.TryGetClipAndVolume(soundName, out var clip, out var volume))
        {
            sfx2DSource.PlayOneShot(clip, volume);
        }
    }
}
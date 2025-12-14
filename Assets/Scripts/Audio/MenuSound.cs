using UnityEngine;

public class MenuSound : MonoBehaviour
{
    [Header("Cài đặt Âm thanh")]
    public AudioSource sfxSource; // Kéo AudioSource vào đây
    public AudioClip clickSound;  // Kéo file tiếng "Click" vào đây

    // Hàm này sẽ được gọi khi bấm nút
    public void PlayClickSound()
    {
        if (sfxSource != null && clickSound != null)
        {
            // PlayOneShot giúp âm thanh không bị ngắt quãng nếu bấm liên tục
            sfxSource.PlayOneShot(clickSound);
        }
    }
}
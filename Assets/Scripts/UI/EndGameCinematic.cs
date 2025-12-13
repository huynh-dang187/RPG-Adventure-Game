using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;
using Cinemachine;

public class EndGameCinematic : MonoBehaviour
{
    [Header("1. Camera Rung")]
    public CinemachineVirtualCamera vCam; 
    public float shakeIntensity = 2.0f;   
    public float shakeTime = 0.5f;        

    [Header("2. Cài đặt Video")]
    public VideoPlayer videoPlayer;   
    public RawImage videoScreen;      

    [Header("3. Cài đặt Scene")]
    public string creditsSceneName = "CreditsScene"; 

    [Header("4. Thời gian")]
    public float waitBeforeVideo = 2.0f; 

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            
            // Khóa Player lại
            var playerController = other.GetComponent<PlayerController>();
            if (playerController != null) playerController.enabled = false;
            var rb = other.GetComponent<Rigidbody2D>();
            if(rb) rb.linearVelocity = Vector2.zero; 
            var anim = other.GetComponent<Animator>();
            if(anim) { anim.SetBool("isMoving", false); }

            StartCoroutine(PlayEndingSequence());
        }
    }

    IEnumerator PlayEndingSequence()
    {
        // 1. TẮT UI GAMEPLAY (Tìm và diệt)
        GameObject liveUI = GameObject.Find("UICanvas"); 
        if (liveUI != null) liveUI.SetActive(false);
        else 
        {
            GameObject liveHUD = GameObject.Find("HUD_Group");
            if (liveHUD != null) liveHUD.SetActive(false);
        }
        
        // 2. RUNG CAMERA
        if (vCam != null)
        {
            var perlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (perlin != null)
            {
                perlin.m_AmplitudeGain = shakeIntensity; 
                yield return new WaitForSeconds(shakeTime); 
                perlin.m_AmplitudeGain = 0f; 
            }
        }
        else yield return new WaitForSeconds(shakeTime);

        // 3. MÀN HÌNH SÁNG DẦN (FADE TO WHITE)
        if (SoundManager.Instance != null) SoundManager.Instance.StopMusic();
        
        if (UIFade.Instance != null) 
        {
             // Đảm bảo object Fade đang bật thì mới chạy được
             UIFade.Instance.gameObject.SetActive(true);
             UIFade.Instance.FadeToWhite(); 
        }

        // Chờ màn hình trắng xóa trong 2 giây (Lúc này màn hình đang Trắng tinh)
        yield return new WaitForSeconds(waitBeforeVideo);

        // 4. CHIẾU VIDEO & VÉN MÀN (QUAN TRỌNG)
        if (videoPlayer != null && videoScreen != null)
        {
            // Bật màn hình Video (lúc này vẫn đang nằm SAU cái màn trắng)
            videoScreen.gameObject.SetActive(true);
            
            videoPlayer.Prepare();
            float timeout = 0;
            while (!videoPlayer.isPrepared && timeout < 5f) 
            {
                timeout += Time.deltaTime;
                yield return null; 
            }
            videoPlayer.Play();

            // --- THÊM DÒNG NÀY: Vén màn trắng ra để lộ Video ---
            if (UIFade.Instance != null) UIFade.Instance.FadeToClear(); 
            // ----------------------------------------------------

            yield return new WaitForSeconds((float)videoPlayer.length);
        }

        // 5. HẾT PHIM -> FADE ĐEN -> CREDITS
        if (UIFade.Instance != null) UIFade.Instance.FadeToBlack(); 
        
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(creditsSceneName);
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIFade : Singleton<UIFade>
{
    [SerializeField] private Image fadeScreen;
    [SerializeField] private float fadeSpeed = 1f;

    private IEnumerator fadeRoutine;

    // --- 1. FADE MÀU ĐEN (Dùng cho chuyển cảnh bình thường) ---
    public void FadeToBlack()
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);

        // Đặt màu của tấm ảnh thành ĐEN (RGB: 0,0,0) nhưng giữ nguyên độ mờ hiện tại
        fadeScreen.color = new Color(0f, 0f, 0f, fadeScreen.color.a);

        fadeRoutine = FadeRoutine(1); // 1 là đặc (không nhìn thấy game)
        StartCoroutine(fadeRoutine);
    }

    // --- 2. FADE MÀU TRẮNG (Dùng cho End Game / Flash) ---
    public void FadeToWhite()
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);

        // Đặt màu của tấm ảnh thành TRẮNG (RGB: 1,1,1) nhưng giữ nguyên độ mờ hiện tại
        fadeScreen.color = new Color(1f, 1f, 1f, fadeScreen.color.a);

        fadeRoutine = FadeRoutine(1); // 1 là trắng xóa
        StartCoroutine(fadeRoutine);
    }

    // --- 3. LÀM TRONG SUỐT (Để hiện game lên) ---
    public void FadeToClear()
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);

        // Không cần đổi màu, chỉ cần giảm Alpha về 0
        fadeRoutine = FadeRoutine(0); 
        StartCoroutine(fadeRoutine);
    }

    // --- LOGIC CHẠY FADE (GIỮ NGUYÊN) ---
    private IEnumerator FadeRoutine(float targetAlpha)
    {
        while (!Mathf.Approximately(fadeScreen.color.a, targetAlpha))
        {
            float alpha = Mathf.MoveTowards(fadeScreen.color.a, targetAlpha, fadeSpeed * Time.deltaTime);
            
            // Cập nhật Alpha mới, giữ nguyên màu RGB hiện tại (dù là Đen hay Trắng)
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, alpha);
            
            yield return null;  
        }
    }
}
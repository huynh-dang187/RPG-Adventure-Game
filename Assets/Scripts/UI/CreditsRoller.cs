using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Dùng TextMeshPro

public class CreditsRoller : MonoBehaviour
{
    [Header("Cài đặt")]
    public float scrollSpeed = 100f; // Tốc độ chạy chữ (chỉnh nhanh chậm ở đây)
    public string mainMenuSceneName = "MenuStart"; // Tên Scene Menu chính để quay về
    
    [Header("Kết nối UI")]
    public RectTransform textRect; // Kéo cái CreditsText vào đây
    public float limitY = 2000f;   // Chữ chạy đến độ cao này thì tự thoát

    void Update()
    {
        // 1. Logic trôi chữ
        if (textRect != null)
        {
            // Cộng thêm vào vị trí Y để nó đi lên
            textRect.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

            // 2. Kiểm tra nếu chạy hết thì về Menu
            if (textRect.anchoredPosition.y > limitY)
            {
                ReturnToMenu();
            }
        }

        // 3. Tính năng SKIP (Bấm nút bất kỳ để bỏ qua)
        if (Input.anyKeyDown)
        {
            ReturnToMenu();
        }
    }

    void ReturnToMenu()
    {
        // Load về lại màn hình Menu
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
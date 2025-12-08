using UnityEngine;
using UnityEngine.SceneManagement; // Thư viện để chuyển cảnh

public class MainMenuController : MonoBehaviour
{
    [Header("Gán cái bảng Tutorial vào đây")]
    public GameObject tutorialPanel; 

    // 1. Logic nút START
    public void StartGame()
    {
        // Đảm bảo tên trong ngoặc kép giống y hệt tên file Scene của bạn
        SceneManager.LoadScene("SceneStart"); 
    }

    // 2. Logic nút HƯỚNG DẪN (Mở bảng)
    public void OpenTutorial()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true); // Hiện bảng lên
        }
    }

    // 3. Logic nút ĐÓNG HƯỚNG DẪN (Tắt bảng)
    public void CloseTutorial()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false); // Ẩn bảng đi
        }
    }

    // 4. Logic nút THOÁT GAME
    public void QuitGame()
    {
        Debug.Log("Đã thoát game!"); // Dòng này để test trong Unity Editor
        Application.Quit(); // Lệnh này chạy khi build ra file .exe
    }
}
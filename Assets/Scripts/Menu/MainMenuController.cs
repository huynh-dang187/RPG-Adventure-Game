using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Loading")]
    public GameObject loadingPanel; 
    public Slider loadingBar;
    
    [Header("Hệ thống Mẹo Game")]
    public TextMeshProUGUI tipText; 
    [TextArea(2, 5)]
    public string[] gameTips; // Nhập nhiều mẹo vào đây

    [Header("Cấu hình Fake Loading")]
    [Tooltip("Thời gian load giả vờ (giây)")]
    public float fakeLoadingTime = 7.0f; 
    [Tooltip("Bao lâu thì đổi mẹo một lần?")]
    public float tipChangeInterval = 2.5f;

    // --- CÁC HÀM CŨ GIỮ NGUYÊN ---
    public GameObject tutorialPanel;
    public void OpenTutorial() { if(tutorialPanel) tutorialPanel.SetActive(true); }
    public void CloseTutorial() { if(tutorialPanel) tutorialPanel.SetActive(false); }
    public void QuitGame() { Application.Quit(); }

    // --- LOGIC START GAME MỚI ---
    public void StartGame()
    {
        StartCoroutine(LoadLevelFakeAsync("SceneStart"));
    }

    IEnumerator LoadLevelFakeAsync(string sceneName)
    {
        if (loadingPanel != null) loadingPanel.SetActive(true);

        // 1. Tải ngầm Scene nhưng KHÔNG cho vào ngay
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // Chặn cửa lại!

        float timer = 0f;
        float tipTimer = 0f;
        
        // Hiện câu mẹo đầu tiên ngay lập tức
        ShowRandomTip();

        // 2. Vòng lặp giả vờ (Chạy trong khoảng fakeLoadingTime)
        while (timer < fakeLoadingTime)
        {
            timer += Time.deltaTime;
            tipTimer += Time.deltaTime;

            // --- A. XỬ LÝ THANH LOADING ---
            // Tính toán tiến độ giả: chạy từ 0 đến 0.9 (90%)
            // Chúng ta chừa lại 10% cuối để khi load xong hẳn mới phi lên
            float fakeProgress = Mathf.Clamp01(timer / fakeLoadingTime) * 0.9f;

            if (loadingBar != null)
            {
                loadingBar.value = fakeProgress;
            }

            // --- B. XỬ LÝ ĐỔI MẸO ---
            if (tipTimer >= tipChangeInterval)
            {
                ShowRandomTip();
                tipTimer = 0f; // Reset đồng hồ đổi mẹo
            }

            // --- C. XỬ LÝ LOAD THẬT ---
            // Nếu máy load xong thật rồi (progress >= 0.9), nó sẽ đứng chờ ở đó
            // Ta không cần làm gì cả, cứ để vòng lặp while chạy hết 7 giây đã.

            yield return null;
        }

        // 3. HẾT GIỜ GIẢ VỜ -> VÀO GAME
        // Đẩy thanh loading lên 100% cho đẹp
        if (loadingBar != null) loadingBar.value = 1f;
        
        // Đợi thêm xíu xiu (0.5s) cho người chơi thấy thanh đầy 100%
        yield return new WaitForSeconds(0.5f);

        // Mở cửa cho vào
        operation.allowSceneActivation = true;
    }

    // Hàm random mẹo tách riêng cho gọn
    void ShowRandomTip()
    {
        if (gameTips.Length > 0 && tipText != null)
        {
            // Chọn ngẫu nhiên nhưng cố gắng không trùng câu vừa hiển thị (nâng cao tí)
            int randomIndex = Random.Range(0, gameTips.Length);
            tipText.text = gameTips[randomIndex];
            
            // Hiệu ứng Fade in/out cho chữ (nếu muốn xịn hơn thì làm sau)
        }
    }
}
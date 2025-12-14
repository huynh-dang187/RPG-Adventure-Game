using UnityEngine;
using UnityEngine.SceneManagement; // Bắt buộc phải có dòng này để check Scene

public class UIHider : MonoBehaviour
{
    [Header("Tên Scene muốn ẩn UI")]
    public string endingSceneName = "Scene5"; // Điền tên Scene 5 của bạn vào đây
    public string menuSceneName = "MenuStart"; // Điền thêm Scene Menu nếu muốn ẩn luôn ở Menu

    private Canvas myCanvas;

    private void Awake()
    {
        myCanvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        // Đăng ký sự kiện: "Mỗi khi chuyển cảnh, hãy gọi hàm OnSceneLoaded"
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Hủy đăng ký khi object bị tắt (để tránh lỗi)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Hàm này sẽ tự chạy mỗi khi sang Scene mới
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (myCanvas == null) return;

        // Kiểm tra tên Scene hiện tại
        if (scene.name == endingSceneName || scene.name == menuSceneName)
        {
            // Nếu là Scene 5 hoặc Menu -> TẮT CANVAS
            myCanvas.enabled = false;
        }
        else
        {
            // Nếu là Scene chơi (1, 2, 3, 4) -> BẬT CANVAS
            myCanvas.enabled = true;
        }
    }
}
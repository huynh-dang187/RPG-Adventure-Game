using UnityEngine;

public class SmokeTrigger : MonoBehaviour
{
    // Kéo thả object dấu chấm than (!) từ trong Player vào đây ở Inspector
    public GameObject exclamationMark; 
    
    // Biến để đảm bảo nó chỉ hiện 1 lần rồi thôi (nếu muốn)
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem cái đi vào có phải là Player không
        if (other.CompareTag("Player") && !hasTriggered)
        {
            ShowAlert();
        }
    }

    void ShowAlert()
    {
        hasTriggered = true; // Đánh dấu là đã chạy rồi

        // 1. Hiện dấu chấm than
        if(exclamationMark != null)
        {
            exclamationMark.SetActive(true);
            
            // Tùy chọn: Tắt dấu chấm than sau 2 giây
            
        }

        // 2. Chỗ này bạn có thể gọi thêm hàm làm mờ màn hình/tăng khói ở đây luôn
        // Example: GameManager.instance.IncreaseFog();
    }

    void HideAlert()
    {
        if(exclamationMark != null)
            exclamationMark.SetActive(false);
    }
}
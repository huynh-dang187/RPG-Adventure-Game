using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [Header("Cài đặt Boss")]
    public MechaGolem_AI bossScript; // Script điều khiển di chuyển/tấn công
    public BossHealth bossHealth;    // <--- ĐÃ SỬA: Dùng trực tiếp BossHealth của bạn
    public GameObject entryDoor;     // Cửa đóng lại

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem có phải Player đi qua không
        if (other.CompareTag("Player") || (other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Player")))
        {
            // 1. Đánh thức Boss
            if (bossScript != null)
            {
                bossScript.WakeUp();
            }

            // 2. HIỆN THANH MÁU
            // Chúng ta sẽ bật trực tiếp cái khung máu trong script BossHealth lên
            // Trong file BossTrigger.cs
            if (bossHealth != null && bossHealth.healthBarFrame != null)
            {
                bossHealth.healthBarFrame.SetActive(true);
                
                // XÓA HOẶC COMMENT DÒNG DƯỚI NÀY ĐI
                // bossHealth.bossNameText.text = "ANCIENT GOLEM";  <-- Xóa dòng này
            }
            else
            {
                Debug.LogWarning("Chưa kéo script BossHealth hoặc Frame máu vào Trigger!");
            }

            // 3. Đóng cửa nhốt Player lại
            if (entryDoor != null)
            {
                entryDoor.SetActive(true);
            }

            // 4. Hủy Trigger
            Destroy(gameObject);
        }
    }
}
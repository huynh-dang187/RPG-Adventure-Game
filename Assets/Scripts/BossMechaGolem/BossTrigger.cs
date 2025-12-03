using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [Header("Cài đặt")]
    public MechaGolem_AI bossScript; // Kéo con Boss vào đây
    public GameObject entryDoor;     // Kéo cánh cửa chặn đường vào (để đóng lại) - Tùy chọn

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem có phải Player đi qua không (dùng attachedRigidbody cho chắc ăn)
        if (other.CompareTag("Player") || (other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Player")))
        {
            // 1. Đánh thức Boss
            if (bossScript != null)
            {
                bossScript.WakeUp();
            }
            else
            {
                Debug.LogError("Chưa kéo Script Boss vào Trigger!");
            }

            // 2. Đóng cửa nhốt Player lại (Nếu có)
            if (entryDoor != null)
            {
                entryDoor.SetActive(true); // Hiện cái cửa lên chặn đường về
            }

            // 3. Hủy chính cái Trigger này để không kích hoạt lại lần nữa
            Destroy(gameObject);
        }
    }
}
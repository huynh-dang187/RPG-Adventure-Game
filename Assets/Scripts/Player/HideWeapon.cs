using UnityEngine;
using UnityEngine.SceneManagement; // Bắt buộc có để check tên Scene

public class HideWeapon : MonoBehaviour
{
    [Header("Cài đặt")]
    [Tooltip("Tên Scene mà bạn muốn giấu vũ khí (VD: Scene5)")]
    [SerializeField] private string targetSceneName = "Scene5"; 
    
    [Tooltip("Tên chính xác của object vũ khí trong Hierarchy")]
    [SerializeField] private string weaponName = "Active Weapon"; 

    // Dùng LateUpdate để ĐÈ LÊN tất cả các lệnh bật vũ khí khác
    void LateUpdate()
    {
        // 1. Chỉ chạy nếu đang ở Scene 5
        if (SceneManager.GetActiveScene().name == targetSceneName)
        {
            // 2. Tìm vũ khí (là con của Player)
            Transform weapon = transform.Find(weaponName);

            if (weapon != null)
            {
                // 3. CƯỠNG CHẾ TẮT
                // Kiểm tra xem nó có đang bật không? Nếu có thì tắt ngay lập tức.
                if (weapon.gameObject.activeSelf)
                {
                    weapon.gameObject.SetActive(false);
                }
                
                // (Kỹ tính hơn) Tắt luôn cái ảnh hiển thị
                SpriteRenderer sr = weapon.GetComponent<SpriteRenderer>();
                if (sr != null && sr.enabled)
                {
                    sr.enabled = false;
                }
            }
        }
    }
}
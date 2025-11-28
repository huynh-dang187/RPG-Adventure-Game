using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    [Header("Sự kiện khi hết quái")]
    public UnityEvent OnAllEnemiesDead;

    private int totalEnemies = 0; // Khởi tạo bằng 0

    void Start()
    {
        // --- SỬA ĐOẠN NÀY ---
        // Thay vì đếm tất cả (childCount), ta dùng vòng lặp để chỉ đếm con nào ĐANG BẬT
        
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf) // Chỉ đếm nếu object đang bật (Active)
            {
                totalEnemies++;
            }
        }
        // --------------------
        
        Debug.Log("Bắt đầu game! Tổng số quái cần diệt: " + totalEnemies);
    }

    public void EnemyDefeated()
    {
        totalEnemies--; 
        
        Debug.Log("Quái chết! Còn lại: " + totalEnemies);

        if (totalEnemies <= 0)
        {
            Debug.Log("Đã diệt hết quái! Mở cổng!");
            OnAllEnemiesDead?.Invoke(); 
        }
    }
}
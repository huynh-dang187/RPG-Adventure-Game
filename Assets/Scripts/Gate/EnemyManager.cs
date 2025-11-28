using UnityEngine;
using UnityEngine.Events; // Để dùng UnityEvent

public class EnemyManager : MonoBehaviour
{
    [Header("Sự kiện khi hết quái")]
    public UnityEvent OnAllEnemiesDead; // Kéo thả sự kiện trong Inspector

    private int totalEnemies;

    void Start()
    {
        // Đếm số lượng con (child) ngay từ đầu
        // Lưu ý: Hàm này chỉ đếm số lượng Child ngay dưới nó
        totalEnemies = transform.childCount;
    }

    public void EnemyDefeated()
    {
        totalEnemies--; // Giảm số lượng đi 1
        
        Debug.Log("Quái chết! Còn lại: " + totalEnemies);

        if (totalEnemies <= 0)
        {
            Debug.Log("Đã diệt hết quái! Mở cổng!");
            OnAllEnemiesDead?.Invoke(); // Kích hoạt sự kiện mở cổng
        }
    }
}
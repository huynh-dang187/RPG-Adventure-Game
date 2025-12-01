using UnityEngine;

public class GolemLaser : MonoBehaviour
{
    public int damage = 10;
    public float lifeTime = 2f; // Thời gian tồn tại của tia laser

    void Start()
    {
        // Tự hủy sau khi bắn xong
        Destroy(gameObject, lifeTime);
    }

    // Dùng OnTriggerStay vì tia laser tồn tại lâu, chạm lâu thì mất máu
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Gọi hàm nhận damage của Player
            // (Bạn có thể thêm cơ chế "mỗi 0.5s mới trừ máu 1 lần" nếu muốn)
            other.GetComponent<PlayerHealth>()?.TakeDamage(damage, transform);
        }
    }
}
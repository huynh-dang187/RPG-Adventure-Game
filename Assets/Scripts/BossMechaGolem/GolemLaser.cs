using UnityEngine;

public class GolemLaser : MonoBehaviour
{
    public int damage = 10;
    public float lifeTime = 2f;

    [Header("Knockback Settings")]
    // Số này càng lớn, Player bay càng xa. Mặc định mình để 15f (khá mạnh)
    public float knockbackThrust = 15f; 

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Gây sát thương
            other.GetComponent<PlayerHealth>()?.TakeDamage(damage, transform);

            // 2. Xử lý ĐẨY LÙI (Phần mới thêm)
            Knockback knockbackScript = other.GetComponent<Knockback>();
            
            // Kiểm tra xem Player có script Knockback không VÀ có đang bị đẩy không
            // (Check !GettingKnockedBack để tránh bị đẩy liên tục hàng nghìn lần mỗi giây gây lỗi game)
            if (knockbackScript != null && !knockbackScript.GettingKnockedBack)
            {
                knockbackScript.GetKnockedBack(transform, knockbackThrust);
            }
        }
    }
}
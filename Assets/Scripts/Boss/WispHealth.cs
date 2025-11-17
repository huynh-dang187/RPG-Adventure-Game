using UnityEngine;

public class WispHealth : MonoBehaviour
{
    // Thêm các tham chiếu này
    private Animator anim;
    private Wisp_AI aiScript;
    private Collider2D coll;
    private bool isDead = false; // Cờ hiệu để tránh chết 2 lần

    void Start()
    {
        // Tự động lấy các component
        anim = GetComponent<Animator>();
        aiScript = GetComponent<Wisp_AI>();
        coll = GetComponent<Collider2D>();
    }

    // Hàm này sẽ được gọi bởi script DamageSource (kiếm)
    public void TakeDamage(int damage, Transform hitTransform)
    {
        // Nếu đã chết rồi thì không làm gì nữa
        if (isDead) return;

        // Đánh dấu là đã chết
        isDead = true; 
        Debug.Log("Wisp đã bị Player chém chết!");

        // 1. Tắt não (để nó dừng bay)
        if (aiScript != null)
        {
            aiScript.enabled = false;
        }

        // 2. Tắt va chạm (để nó không gây damage khi đang chết)
        if (coll != null)
        {
            coll.enabled = false;
        }

        // 3. Kích hoạt animation "Die"
        if (anim != null)
        {
            anim.SetTrigger("Die");
        }

        // 4. Hủy GameObject SAU 1 giây (hoặc thời gian anim Die)
        // (Thay 1f bằng độ dài animation "chết" của bạn)
        Destroy(gameObject, 1f); 
    }
}
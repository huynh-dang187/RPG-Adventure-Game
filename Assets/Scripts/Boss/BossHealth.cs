using UnityEngine;
using UnityEngine.UI; // Sẽ cần nếu bạn dùng thanh máu UI

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 1000;
    public int currentHealth; // <--- DÒNG NÀY SẼ SỬA LỖI CỦA BẠN

    public Reaper_AI aiScript; // Tham chiếu đến script AI
    public Animator animator;
    // public Image healthBar; // Nếu có thanh máu UI, bỏ comment dòng này

    void Start()
    {
        currentHealth = maxHealth; // Đặt máu đầy khi bắt đầu
        
        // Tự động lấy các component
        aiScript = GetComponent<Reaper_AI>();
        animator = GetComponent<Animator>();
    }

    // Hàm này sẽ được Player gọi khi tấn công Boss
    public void TakeDamage(int damage)
    {
        // Nếu boss đã chết, không nhận thêm sát thương
        if (aiScript.currentState == Reaper_AI.BossState.Dead) return;

        currentHealth -= damage;
        
        // Cập nhật thanh máu UI (nếu có)
        // healthBar.fillAmount = (float)currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            currentHealth = 0; // Tránh máu âm
            Die();
        }
        else
        {
            // Có thể thêm anim "Hurt" (bị đánh) ở đây nếu muốn
            // animator.SetTrigger("Hurt");
        }
    }

    void Die()
    {
        Debug.Log("Boss đã chết!");
        
        // 1. Chuyển trạng thái AI sang Dead
        aiScript.currentState = Reaper_AI.BossState.Dead;
        
        // 2. Bóp cò "Die" trong Animator
        animator.SetTrigger("Die");

        // 3. Tắt va chạm để Player đi xuyên qua
        GetComponent<Collider2D>().enabled = false;
        
        // 4. Tắt Rigidbody (nếu cần)
        // GetComponent<Rigidbody2D>().simulated = false;

        // 5. Tắt script AI để boss dừng mọi hành động
        aiScript.enabled = false;

        // 6. Hủy GameObject boss sau 5 giây (để chạy xong anim chết)
        Destroy(gameObject, 5f); 
    }
}
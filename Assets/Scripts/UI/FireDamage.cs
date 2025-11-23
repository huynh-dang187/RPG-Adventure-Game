using UnityEngine;

public class FireDamage : MonoBehaviour
{
    public float knockbackThrust = 10f;
    public int damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // 1. Xử lý Đẩy lùi (Knockback)
            var playerKnockback = other.GetComponent<Knockback>();
            if (playerKnockback != null)
            {
                playerKnockback.GetKnockedBack(this.transform, knockbackThrust);
            }

            // 2. Xử lý Trừ máu + Animation Hurt
            var playerHealth = other.GetComponent<PlayerHealth>(); 
            
            if (playerHealth != null)
            {
                // SỬA Ở ĐÂY: Thêm "this.transform" vào làm tham số thứ 2
                playerHealth.TakeDamage(damageAmount, this.transform); 
            }
        }
    }
}
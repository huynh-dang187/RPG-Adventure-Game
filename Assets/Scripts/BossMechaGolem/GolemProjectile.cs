using UnityEngine;

public class GolemProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 20;
    public float lifeTime = 3f;

    private Vector2 direction;
    private bool isFired = false;

    public void Fire(Vector2 dir)
    {
        direction = dir;
        isFired = true;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (isFired)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // --- QUAN TRỌNG: BỎ QUA BOSS VÀ TRIGGER KHÁC ---
        // Nếu đạn chạm trúng vật thể có Tag "Enemy" (chính nó) hoặc là Trigger (vùng cảm ứng) thì kệ nó
        if (other.CompareTag("Enemy") || other.isTrigger) return;

        // 1. Kiểm tra va chạm với Player
        if (other.CompareTag("Player"))
        {
            // ... (Giữ nguyên code gây dame của bạn) ...
            var health = other.GetComponent<PlayerHealth>();
            if (health != null) health.TakeDamage(damage, transform);
            
            Destroy(gameObject);
        }
        // 2. Chỉ hủy khi chạm vào TƯỜNG (Cần set Layer hoặc Tag cho tường)
        // Thay vì check "Default" (quá chung chung), hãy check cụ thể
        else if (other.CompareTag("Wall") || other.gameObject.layer == LayerMask.NameToLayer("Obstacle")) 
        {
            Destroy(gameObject);
        }
    }
}
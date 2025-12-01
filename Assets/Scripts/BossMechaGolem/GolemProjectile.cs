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
        // 1. Kiểm tra va chạm với Player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Đạn trúng Player!");

            // 2. Gọi hàm gây dame (Cần truyền thêm 'transform' để tính hướng đẩy lùi)
            var health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage, transform); 
            }
            
            // 3. Nổ đạn
            Destroy(gameObject);
        }
        // 4. Kiểm tra va chạm với Tường (Để đạn không bay xuyên tường)
        else if (other.gameObject.layer == LayerMask.NameToLayer("Default")) 
        {
            Destroy(gameObject);
        }
    }
}
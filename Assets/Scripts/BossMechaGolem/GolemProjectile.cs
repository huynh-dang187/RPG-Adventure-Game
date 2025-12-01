using UnityEngine;

public class GolemProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 20;
    public float lifeTime = 3f; // Thời gian tồn tại

    private Vector2 direction;
    private bool isFired = false;

    // Hàm này để Boss gọi khi bắn
    public void Fire(Vector2 dir)
    {
        direction = dir;
        isFired = true;
        
        // Xoay viên đạn theo hướng bắn (nếu cần)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Destroy(gameObject, lifeTime); // Tự hủy sau 3s
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
        if (other.CompareTag("Player"))
        {
            // Gây damage cho Player (Giả sử player có script PlayerHealth)
            // other.GetComponent<PlayerHealth>().TakeDamage(damage); 
            Debug.Log("Trúng Player! Gây " + damage + " sát thương.");
            Destroy(gameObject); // Trúng thì nổ
        }
        else if (other.CompareTag("Ground") || other.CompareTag("Wall"))
        {
            Destroy(gameObject); // Trúng tường cũng nổ
        }
    }
}
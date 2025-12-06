using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;
    [SerializeField] private bool isEnemyProjectile = false;
    [SerializeField] private float projectileRange = 10f; // Biến tầm bắn mặc định

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }

    // --- HÀM 1: Dùng cho Enemy (Shooter) ---
    public void UpdateMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    // --- HÀM 2: Dùng cho Cung (Bow) - BẠN ĐANG THIẾU CÁI NÀY ---
    public void UpdateProjectileRange(float range)
    {
        this.projectileRange = range;
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    private void DetectFireDistance()
    {
        if (Vector3.Distance(transform.position, startPos) > projectileRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Logic xử lý va chạm (Giữ nguyên như cũ)
        // Nếu là đạn của Player
        if (!isEnemyProjectile) 
        {
             // Trúng Enemy
             EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
             if(enemyHealth) { enemyHealth.TakeDamage(1); Destroy(gameObject); }
        }
        else // Nếu là đạn của Enemy
        {
             // Trúng Player
             if (other.CompareTag("Player"))
             {
                 PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                 if (playerHealth) playerHealth.TakeDamage(1, transform);
                 Destroy(gameObject);
             }
        }

        // Trúng tường thì nổ
        if (!other.isTrigger && (other.CompareTag("Wall") || other.gameObject.layer == LayerMask.NameToLayer("Default")))
        {
            Destroy(gameObject);
        }
    }
}
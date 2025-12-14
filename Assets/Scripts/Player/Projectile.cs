using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;
    [SerializeField] private bool isEnemyProjectile = false;
    [SerializeField] private float projectileRange = 10f;

    // --- THÊM DÒNG NÀY ĐỂ CHỈNH DAME ---
    [Header("Chỉnh Sát Thương")]
    [SerializeField] private int damageAmount = 10; // Mặc định là 10, chỉnh trong Unity
    // -----------------------------------

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

    public void UpdateMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

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
        // 1. NẾU LÀ ĐẠN CỦA PLAYER (Bắn trúng quái)
        if (!isEnemyProjectile) 
        {
             // Tìm EnemyHealth (Quái thường)
             EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
             if(enemyHealth) { 
                 // Sửa số 1 thành damageAmount
                 enemyHealth.TakeDamage(damageAmount); 
                 Destroy(gameObject); 
                 return; // Dừng luôn
             }

             // Tìm BossHealth (Nếu bắn trúng Boss) - Thêm cái này cho chắc
             BossHealth bossHealth = other.GetComponent<BossHealth>();
             if(bossHealth) {
                 bossHealth.TakeDamage(damageAmount, transform);
                 Destroy(gameObject);
                 return;
             }
        }
        // 2. NẾU LÀ ĐẠN CỦA ENEMY (Bắn trúng Player)
        else 
        {
             if (other.CompareTag("Player"))
             {
                 PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                 // --- SỬA Ở ĐÂY: Thay số 1 thành damageAmount ---
                 if (playerHealth) playerHealth.TakeDamage(damageAmount, transform);
                 
                 Destroy(gameObject);
             }
        }

        // Xử lý va chạm tường
        if (!other.isTrigger && (other.CompareTag("Wall") || other.gameObject.layer == LayerMask.NameToLayer("Default")))
        {
            Destroy(gameObject);
        }
    }
}
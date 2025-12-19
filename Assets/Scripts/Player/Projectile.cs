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
        // 1. NẾU LÀ ĐẠN CỦA PLAYER (Bắn trúng quái/Boss)
        if (!isEnemyProjectile) 
        {
             // --- BƯỚC 1: TÌM MÁU CỦA QUÁI THƯỜNG ---
             EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
             if (enemyHealth != null) { 
                 enemyHealth.TakeDamage(damageAmount); 
                 Destroy(gameObject); 
                 return; // Xong việc thì nghỉ
             }

             // --- BƯỚC 2: TÌM MÁU CỦA BOSS (QUAN TRỌNG) ---
             // Thử tìm ngay trên chỗ va chạm
             BossHealth bossHealth = other.GetComponent<BossHealth>();
             
             // Nếu không thấy, thử tìm ngược lên CAO HƠN (Object cha)
             // Lệnh này giúp bắn vào tay chân Boss vẫn dính dame
             if (bossHealth == null) {
                 bossHealth = other.GetComponentInParent<BossHealth>();
             }

             if (bossHealth != null) {
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
                 if (playerHealth) playerHealth.TakeDamage(damageAmount, transform);
                 Destroy(gameObject);
             }
        }

        // Xử lý va chạm tường (Trừ khi là Trigger)
        if (!other.isTrigger && (other.CompareTag("Wall") || other.gameObject.layer == LayerMask.NameToLayer("Default")))
        {
            Destroy(gameObject);
        }
    }
}
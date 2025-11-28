using UnityEngine;

public class SkeletonAI : MonoBehaviour
{
    [Header("Cài đặt chỉ số")]
    public float moveSpeed = 2f;        
    public float detectRange = 5f;      
    public float attackRange = 1f;      
    public float attackCooldown = 2f;   
    public int damage = 1;              

    [Header("References")]
    private Animator animator;
    private Rigidbody2D rb;
    private float nextAttackTime = 0f;
    private Vector3 initScale;          

    // --- 1. THÊM BIẾN KNOCKBACK ---
    private SkeletonKnockback knockback; 

    void Start()
    {
        animator = GetComponentInChildren<Animator>(); // Tìm ở con cho chắc
        rb = GetComponent<Rigidbody2D>();
        
        // --- 2. LẤY COMPONENT KNOCKBACK ---
        knockback = GetComponent<SkeletonKnockback>();

        initScale = transform.localScale;
    }

    void Update()
    {
        // --- 3. QUAN TRỌNG: CHẶN DI CHUYỂN KHI BỊ ĐẨY ---
        // Nếu đang bị đẩy lùi -> Dừng hàm Update luôn, không cho tính toán di chuyển nữa
        if (knockback != null && knockback.GettingKnockedBack) 
        {
            return; 
        }
        // ------------------------------------------------

        if (PlayerHealth.Instance == null) return;
        if (PlayerHealth.Instance.isDead) 
        {
            StopMoving();
            return;
        }

        Transform playerTransform = PlayerHealth.Instance.transform;
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        // LOGIC TRẠNG THÁI
        if (distance <= attackRange)
        {
            StopMoving();
            if (Time.time >= nextAttackTime)
            {
                StartAttackAnimation();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else if (distance <= detectRange)
        {
            ChasePlayer(playerTransform);
        }
        else
        {
            StopMoving();
        }
    }

    void ChasePlayer(Transform target)
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed; 
        animator.SetBool("isRunning", true);
        FlipSprite(direction.x);
    }

    void StopMoving()
    {
        rb.linearVelocity = Vector2.zero;
        animator.SetBool("isRunning", false);
    }

    void StartAttackAnimation()
    {
        animator.SetTrigger("attack");
    }

    public void DealDamage()
    {
        if (PlayerHealth.Instance != null && !PlayerHealth.Instance.isDead)
        {
            float distance = Vector2.Distance(transform.position, PlayerHealth.Instance.transform.position);
            if (distance <= attackRange + 0.5f)
            {
                PlayerHealth.Instance.TakeDamage(damage, transform);
            }
        }
    }

    void FlipSprite(float xDirection)
    {
        if (xDirection > 0)
            transform.localScale = new Vector3(Mathf.Abs(initScale.x), initScale.y, initScale.z);
        else if (xDirection < 0)
            transform.localScale = new Vector3(-Mathf.Abs(initScale.x), initScale.y, initScale.z);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
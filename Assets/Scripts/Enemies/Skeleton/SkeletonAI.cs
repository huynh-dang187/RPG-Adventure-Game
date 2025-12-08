using UnityEngine;

public class SkeletonAI : MonoBehaviour
{
    [Header("Cài đặt chỉ số chiến đấu")]
    public float moveSpeed = 2f;        // Tốc độ khi đuổi theo
    public float detectRange = 5f;      // Phạm vi phát hiện người chơi
    public float attackRange = 1f;      // Phạm vi tấn công
    public float attackCooldown = 2f;   
    public int damage = 1;              

    // --- [MỚI] CÀI ĐẶT TUẦN TRA (ROAMING) ---
    [Header("Cài đặt Tuần Tra")]
    public float roamingRange = 4f;      // Phạm vi đi tuần tính từ điểm xuất phát
    public float roamingSpeed = 1f;      // Tốc độ khi đi tuần (nên chậm hơn khi đuổi)
    public float startWaitTime = 2f;     // Thời gian đứng nghỉ khi đến đích

    private Vector3 startPosition;       // Lưu vị trí spawn ban đầu để quái không đi quá xa
    private Vector3 roamPosition;        // Điểm đến ngẫu nhiên tiếp theo
    private float waitTimer;             // Bộ đếm thời gian nghỉ
    // ---------------------------------------

    [Header("References")]
    private Animator animator;
    private Rigidbody2D rb;
    private float nextAttackTime = 0f;
    private Vector3 initScale;          

    // Biến cho Knockback (giữ nguyên logic cũ của bạn)
    private SkeletonKnockback knockback; 

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        knockback = GetComponent<SkeletonKnockback>();
        initScale = transform.localScale;

        // --- [MỚI] KHỞI TẠO ĐIỂM TUẦN TRA ---
        startPosition = transform.position;  // Lưu vị trí lúc sinh ra làm tâm
        roamPosition = GetRoamingPosition(); // Tìm điểm đi tuần đầu tiên
        waitTimer = startWaitTime;
    }

    void Update()
    {
        // 1. Nếu đang bị đẩy lùi (Knockback) thì dừng mọi việc lại
        if (knockback != null && knockback.GettingKnockedBack) return; 

        // 2. Kiểm tra Player
        if (PlayerHealth.Instance == null) return;
        if (PlayerHealth.Instance.isDead) 
        {
            StopMoving();
            return;
        }

        Transform playerTransform = PlayerHealth.Instance.transform;
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        // --- LOGIC TRẠNG THÁI AI ---
        
        if (distance <= attackRange) // TRẠNG THÁI: TẤN CÔNG
        {
            StopMoving();
            if (Time.time >= nextAttackTime)
            {
                StartAttackAnimation();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else if (distance <= detectRange) // TRẠNG THÁI: ĐUỔI THEO
        {
            // Reset timer để nếu lát nữa mất dấu người chơi, nó sẽ đứng nghỉ 1 chút rồi mới đi tuần
            waitTimer = startWaitTime; 
            
            // Đuổi theo Player
            ChasePlayer(playerTransform);
        }
        else // TRẠNG THÁI: ĐI TUẦN (MỚI)
        {
            // Nếu Player ở xa quá, thay vì đứng yên -> Gọi hàm đi tuần
            Patrol(); 
        }
    }

    // --- [MỚI] HÀM XỬ LÝ ĐI TUẦN ---
    void Patrol()
    {
        // Kiểm tra xem đã đi đến điểm mục tiêu chưa (khoảng cách < 0.2f)
        if (Vector2.Distance(transform.position, roamPosition) < 0.2f)
        {
            // Đã đến nơi -> Đứng lại nghỉ ngơi
            StopMoving();
            
            if (waitTimer <= 0)
            {
                // Hết giờ nghỉ -> Tìm điểm mới và Reset thời gian nghỉ
                roamPosition = GetRoamingPosition();
                waitTimer = startWaitTime;
            }
            else
            {
                // Đang đếm ngược thời gian nghỉ
                waitTimer -= Time.deltaTime;
            }
        }
        else
        {
            // Chưa đến nơi -> Tiếp tục đi tới điểm roamPosition
            Vector2 direction = (roamPosition - transform.position).normalized;
            
            // Dùng roamingSpeed (chậm hơn) để đi thong thả
            rb.linearVelocity = direction * roamingSpeed; 
            
            animator.SetBool("isRunning", true);
            FlipSprite(direction.x);
        }
    }

    // --- [MỚI] TÌM ĐIỂM NGẪU NHIÊN TRONG PHẠM VI ---
    Vector3 GetRoamingPosition()
    {
        // Random.insideUnitCircle trả về 1 điểm ngẫu nhiên trong hình tròn bán kính 1
        // Ta nhân với roamingRange và cộng với vị trí gốc (startPosition) để lấy điểm thực tế
        return startPosition + (Vector3)Random.insideUnitCircle * roamingRange;
    }

    // --- CÁC HÀM CŨ (GIỮ NGUYÊN) ---
    void ChasePlayer(Transform target)
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed; // Đuổi theo thì dùng tốc độ nhanh (moveSpeed)
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

        // [MỚI] Vẽ thêm vòng tròn màu xanh lá để bạn thấy phạm vi đi tuần
        Gizmos.color = Color.green;
        Vector3 drawCenter = Application.isPlaying ? startPosition : transform.position;
        Gizmos.DrawWireSphere(drawCenter, roamingRange);
    }
}
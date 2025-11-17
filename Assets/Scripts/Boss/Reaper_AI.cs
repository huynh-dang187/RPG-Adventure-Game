using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper_AI : MonoBehaviour
{
    [Header("Attack Settings")]
    public Transform attackPoint;     // Kéo AttackPoint rỗng vào đây
    public float attackRadius = 1.5f;   // Độ rộng của cú chém
    public int attackDamage = 10;       // Sát thương
    public LayerMask playerLayer;     // Để boss chỉ chém Player, ko chém đệ
    // 1. Định nghĩa các trạng thái
    // (Bạn có thể thêm bớt nếu cần, ví dụ: Skill1, Hurt)
    public enum BossState { Idle, Chasing, Attacking, Summoning, Dead }
    public BossState currentState; // Trạng thái hiện tại của Boss

    // 2. Các biến tham chiếu (Component)
    // Kéo thả vào Inspector hoặc để code tự tìm
    public Transform player;
    public Animator animator;
    public BossHealth bossHealth; // Script BossHealth.cs của bạn
    public SpriteRenderer spriteRenderer; // Dùng để lật mặt boss

    // 3. Thông số AI (Tinh chỉnh trong Inspector)
    [Header("AI Parameters")]
    public float moveSpeed = 3f;
    public float detectionRange = 15f; // Tầm phát hiện Player
    public float attackRange = 2.5f;   // Tầm đánh cận chiến
    public float summonRange = 10f;  // Tầm để gọi đệ (phải xa hơn attackRange)

    // 4. Cooldown kỹ năng (Thời gian hồi chiêu)
    [Header("Skill Cooldowns")]
    public float timeBetweenSummons = 10f; // 10 giây gọi đệ 1 lần
    private float lastSummonTime; // Lần cuối gọi đệ là khi nào

    // 5. Coroutine Check (Để tránh bug gọi 2 chiêu cùng lúc)
    private bool isActionActive = false; // Cờ hiệu, True = đang đánh/sumon

    // HÀM KHỞI ĐỘNG (Chạy 1 lần đầu tiên)
    void Start()
    {
        // Lấy các component trên chính GameObject này
        animator = GetComponent<Animator>();
        bossHealth = GetComponent<BossHealth>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Tìm Player bằng Tag.
        // **QUAN TRỌNG:** Hãy đảm bảo Player của bạn được gán Tag là "Player"
        try
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        catch (System.Exception)
        {
            Debug.LogError("KHÔNG TÌM THẤY PLAYER! Bạn đã gán Tag 'Player' cho Player chưa?");
        }
        
        // Trạng thái ban đầu
        currentState = BossState.Idle;
        // Đặt thời gian này về âm để boss có thể summon ngay lập tức khi vào trận
        lastSummonTime = -timeBetweenSummons; 
    }

    // HÀM UPDATE (Chạy mỗi frame)
    void Update()
    {
        // ----------------- CÁC ĐIỀU KIỆN "CHẾT" -----------------
        // Nếu Player chết, hoặc Boss chết, hoặc không tìm thấy Player -> Đứng yên
        if (player == null || currentState == BossState.Dead || bossHealth.currentHealth <= 0)
        {
            return; // Dừng hàm Update tại đây
        }

        // Nếu đang bận (đang Attack/Summon), cũng không làm gì trong Update
        if (isActionActive)
        {
            return;
        }

        // ----------------- MÁY TRẠNG THÁI (STATE MACHINE) -----------------
        
        // Tính khoảng cách tới Player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Lật mặt Boss (luôn luôn)
        FacePlayer();

        // Kiểm tra trạng thái hiện tại của Boss
        switch (currentState)
        {
            // --- TRẠNG THÁI 1: IDLE (Đứng yên) ---
            case BossState.Idle:
                // Nếu thấy Player trong tầm phát hiện, rượt theo
                if (distanceToPlayer < detectionRange)
                {
                    currentState = BossState.Chasing;
                }
                break;

            // --- TRẠNG THÁI 2: CHASING (Rượt đuổi) ---
            case BossState.Chasing:
                // Di chuyển về phía Player
                // Dùng .position của player nhưng giữ Y của boss (nếu boss chỉ đi ngang)
                // Hoặc di chuyển cả 2 trục nếu là game top-down
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

                // Quyết định hành động dựa trên khoảng cách
                
                // Ưu tiên 1: Tấn công (nếu đủ gần)
                if (distanceToPlayer <= attackRange)
                {
                    StartCoroutine(Attack()); // Chạy hàm Attack
                }
                // Ưu tiên 2: Gọi đệ (nếu đủ tầm và hồi chiêu xong)
                else if (distanceToPlayer <= summonRange && Time.time > lastSummonTime + timeBetweenSummons)
                {
                    StartCoroutine(Summon()); // Chạy hàm Summon
                }
                // (Nếu bạn có skill1, thêm 1 cái "else if" nữa ở đây)

                break;

            // (Các trạng thái Attacking, Summoning, Dead không cần code trong Update,
            // vì chúng được quản lý bởi Coroutine)
        }
    }

    // HÀM LẬT MẶT BOSS
    void FacePlayer()
    {
        if (transform.position.x < player.position.x)
        {
            // Player đang ở bên phải Boss -> Boss quay phải
            spriteRenderer.flipX = false;
        }
        else
        {
            // Player đang ở bên trái Boss -> Boss quay trái
            spriteRenderer.flipX = true;
        }
    }

    // COROUTINE TẤN CÔNG (Hàm chạy song song)
    IEnumerator Attack()
    {
        isActionActive = true; // Báo hiệu: "Tôi đang bận đánh!"
        currentState = BossState.Attacking; // Chuyển trạng thái
        animator.SetTrigger("DoAttack");     // Bóp cò "DoAttack" trong Animator

        // Chờ animation chạy
        // **CHỈNH SỐ NÀY:** (ví dụ 1.2 giây) sao cho khớp với thời gian
        // chạy thực tế của animation "attack" của bạn.
        yield return new WaitForSeconds(1.2f); 

        // Đánh xong
        isActionActive = false; // Báo hiệu: "Tôi rảnh rồi!"
        currentState = BossState.Chasing; // Quay lại rượt đuổi
    }

    // COROUTINE TRIỆU HỒI
    IEnumerator Summon()
    {
        isActionActive = true; // Báo hiệu: "Tôi đang bận triệu hồi!"
        lastSummonTime = Time.time; // Đặt lại thời gian hồi chiêu
        currentState = BossState.Summoning;
        animator.SetTrigger("DoSummon");

        // Chờ animation chạy
        // **CHỈNH SỐ NÀY:** (ví dụ 2 giây) cho khớp với anim "summon"
        yield return new WaitForSeconds(2.0f); 

        // Triệu hồi xong
        isActionActive = false; // Báo hiệu: "Tôi rảnh rồi!"
        currentState = BossState.Chasing;
    }


    // -----------------------------------------------------------------
    // CÁC HÀM RỖNG ĐỂ CHỜ BƯỚC 5 VÀ 6 (Sẽ được gọi bằng ANIMATION EVENT)
    // -----------------------------------------------------------------
    
    // Hàm này sẽ được gọi bằng Event trên anim "attack"
    public void DealDamage_Attack() 
    {
        // 1. Tạo một vòng tròn vô hình tại AttackPoint
    // Nó sẽ trả về MỌI collider mà nó chạm vào
    Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, playerLayer);

    // 2. Duyệt qua tất cả những thứ vừa va chạm
    foreach (Collider2D player in hitPlayers)
    {
        // 3. Nếu trúng Player (đã lọc bằng LayerMask, nhưng kiểm tra
        // cho chắc), thì gây sát thương
        Debug.Log("Boss đã chém trúng Player!");

        // Giả sử Player có script PlayerHealth và hàm TakeDamage
        // (Bạn phải tự tạo script này cho Player của mình nhé)
        player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
    }
    }

    // Hàm này sẽ được gọi bằng Event trên anim "summon"
    public void ExecuteSummon() 
    { 
        // Sẽ code ở Bước 6
        Debug.Log("BOSS TRIỆU HỒI ĐỆ!");
    }
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
    // -----------------------------------------------------------------
}
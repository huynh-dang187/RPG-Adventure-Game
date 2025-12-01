using UnityEngine;

public class MechaGolem_AI : MonoBehaviour
{
    public enum BossState { Idle, MeleeAttack, LaserAttack, Shield, Dead }
    public BossState currentState;

    [Header("Components")]
    public Transform player;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
[Header("Projectile Settings")] // <--- THÊM TIÊU ĐỀ NÀY
    public GameObject projectilePrefab; // <--- Kéo Prefab GolemArmBullet vào đây
    public Transform firePoint;         // <--- Vị trí viên đạn bay ra (tạo 1 cái empty object ở tay boss)
    [Header("Stats")]
    public float meleeRange = 3f;   // Tầm đấm
    public float shootRange = 10f;  // Tầm bắn tay
    public float laserRange = 15f;  // Tầm bắn laser

    [Header("Cooldowns")]
    public float actionCooldown = 2f; // Thời gian nghỉ giữa các chiêu
    private float lastActionTime;
// ... (Các biến cũ) ...

    [Header("Enrage Settings")]
    public bool isEnraged = false; // Biến kiểm tra xem đã nổi giận chưa
    private BossHealth bossHealth; // Tham chiếu để lấy máu

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        // Tự lấy script máu trên người nó
        bossHealth = GetComponent<BossHealth>(); 
        
        currentState = BossState.Idle;
    }

    void Update()
    {
        if (player == null || currentState == BossState.Dead) return;

        // --- LOGIC MỚI: KIỂM TRA NỔI GIẬN ---
        // Nếu chưa nổi giận VÀ Máu tụt xuống dưới 50%
        if (!isEnraged && bossHealth.currentHealth < (bossHealth.maxHealth * 0.5f))
        {
            StartCoroutine(EnrageRoutine());
            return; // Dừng các hành động khác để ưu tiên gồng
        }
        // -------------------------------------

        FacePlayer();

        if (Time.time > lastActionTime + actionCooldown && currentState == BossState.Idle)
        {
            DecideNextMove();
        }
    }

    // Coroutine cho hành động Glow/Nổi giận
    System.Collections.IEnumerator EnrageRoutine()
    {
        isEnraged = true; // Đánh dấu đã nổi giận (để không gồng lại lần 2)
        currentState = BossState.Idle; // Tạm coi là Idle để không bị kẹt logic

        Debug.Log("BOSS NỔI GIẬN!");
        animator.SetTrigger("Enrage"); // Chạy anim Glow

        // Tăng sức mạnh (Ví dụ)
        actionCooldown = 1.0f; // Hồi chiêu nhanh hơn (đánh dồn dập hơn)
        
        // (Tùy chọn) Đổi màu boss sang hơi đỏ để người chơi biết
        spriteRenderer.color = new Color(1f, 0.7f, 0.7f); 

        // Chờ chạy hết anim Glow (ví dụ anim dài 1.5s)
        yield return new WaitForSeconds(1.5f);
        
        // Sau khi gồng xong, reset thời gian để đánh ngay lập tức
        lastActionTime = Time.time; 
    }
    

    void FacePlayer()
    {
        // Code lật mặt (Flip) giống Reaper
        if (player.position.x > transform.position.x)
            spriteRenderer.flipX = false; // Phải
        else
            spriteRenderer.flipX = true;  // Trái
    }

    void DecideNextMove()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= meleeRange)
        {
            StartCoroutine(MeleeAttack());
        }
        else
        {
            // Logic chọn chiêu ngẫu nhiên
            // Nếu ĐÃ NỔI GIẬN (isEnraged = true) thì tỉ lệ bắn Laser cao hơn
            // Hoặc chỉ khi nổi giận mới được bắn Laser
            
            if (isEnraged) 
            {
                // Khi nổi giận: 50% bắn tay, 50% bắn Laser
                int rng = Random.Range(0, 2);
                if (rng == 0) StartCoroutine(ShootArmProjectile());
                else StartCoroutine(LaserBeamAttack());
            }
            else
            {
                // Chưa nổi giận: Chỉ bắn tay thôi
                StartCoroutine(ShootArmProjectile());
            }
        }
    }

    // --- CÁC HÀM TẤN CÔNG (Sẽ điền chi tiết sau) ---
    
    System.Collections.IEnumerator MeleeAttack()
    {
        currentState = BossState.MeleeAttack;
        lastActionTime = Time.time; // Reset cooldown
        
        // 1. Lướt tới (Dash)
        // 2. Chạy Anim Melee
        animator.SetTrigger("Melee");
        
        yield return new WaitForSeconds(1.5f); // Chờ anim xong
        currentState = BossState.Idle;
    }

    System.Collections.IEnumerator ShootArmProjectile()
    {
        currentState = BossState.LaserAttack; // (Hoặc tạo state Shoot riêng nếu muốn)
        lastActionTime = Time.time;

        // 1. Chạy Anim Shoot
        animator.SetTrigger("Shoot");

        // 2. Chờ 1 chút cho tay giơ lên (ví dụ 0.5s) - Canh theo animation
        yield return new WaitForSeconds(0.5f);

        // 3. Tạo đạn
        if (projectilePrefab != null && firePoint != null)
        {
            // Tạo viên đạn tại vị trí firePoint
            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            
            // Lấy script đạn
            GolemProjectile script = bullet.GetComponent<GolemProjectile>();
            
            // Tính hướng bắn (từ Boss tới Player)
            Vector2 dir = (player.position - transform.position).normalized;
            
            // Ra lệnh bắn
            script.Fire(dir);
        }

        // 4. Chờ Anim kết thúc
        yield return new WaitForSeconds(1f); 
        currentState = BossState.Idle;
    }

    System.Collections.IEnumerator LaserBeamAttack()
    {
        // Bắn Laser hủy diệt
        Debug.Log("Bắn Laser!");
        lastActionTime = Time.time + 2f; // Laser tốn nhiều sức hơn
        yield return null;
    }
}
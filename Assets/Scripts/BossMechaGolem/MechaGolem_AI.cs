using UnityEngine;
using System.Collections;

public class MechaGolem_AI : MonoBehaviour
{
    public enum BossState { Sleeping, Idle, MeleeAttack, LaserAttack, Shield, Dead }
    public BossState currentState;

    [Header("Components")]
    public Transform player;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Laser Settings")]
    public GameObject laserPrefab;

    [Header("Melee Settings")]
    public Transform meleePoint;
    public float meleeRadius = 2f;
    public int meleeDamage = 1; // Đã sửa về 1 cho hợp lý với 3 máu

    [Header("Stats")]
    public float moveSpeed = 4f; // Sửa lại theo ảnh bạn gửi
    public float dashSpeed = 15f;
    public float dashRange = 5f;
    public float shootRange = 10f;
    // public float laserRange = 15f; // Tạm không dùng

    [Header("Cooldowns")]
    public float actionCooldown = 2f;
    private float lastActionTime;

    [Header("Enrage Settings")]
    public bool isEnraged = false;
    private BossHealth bossHealth;
    private GameObject currentLaser;

    // --- LOGIC GÂY SÁT THƯƠNG CẬN CHIẾN (Gắn vào Animation Event) ---
    public void CheckMeleeHit()
    {
        // Quét tất cả layer để chắc chắn trúng Player dù Player đang ở Layer nào
        Collider2D[] hits = Physics2D.OverlapCircleAll(meleePoint.position, meleeRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Debug.Log("BOSS ĐẤM TRÚNG PLAYER!");
                hit.GetComponent<PlayerHealth>()?.TakeDamage(meleeDamage, transform);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (meleePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(meleePoint.position, meleeRadius);
        }
    }

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        else Debug.LogWarning("Boss không tìm thấy Player!");

        bossHealth = GetComponent<BossHealth>();
        currentState = BossState.Sleeping;
    }

    void Update()
    {
        if (player == null || currentState == BossState.Dead) return;
        if (currentState == BossState.Sleeping) return;

        // Nếu còn ít máu thì hóa điên
        if (!isEnraged && bossHealth.currentHealth < (bossHealth.maxHealth * 0.5f))
        {
            StartCoroutine(EnrageRoutine());
            return;
        }

        // Chỉ di chuyển khi ĐANG RẢNH (Idle)
        if (currentState == BossState.Idle)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            // Luôn đi bộ theo Player nếu ở xa hơn tầm dừng (ví dụ 3.0f)
            if (distance > 3.0f)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            }

            FacePlayer();

            // Hồi chiêu xong thì ra quyết định
            if (Time.time > lastActionTime + actionCooldown)
            {
                DecideNextMove();
            }
        }
    }

    public void WakeUp()
    {
        if (currentState == BossState.Sleeping)
        {
            Debug.Log("BOSS TỈNH GIẤC!");
            currentState = BossState.Idle;
            lastActionTime = Time.time + 1f;
        }
    }

    void FacePlayer()
    {
        // Chỉ quay mặt khi Rảnh hoặc đang chạy
        if (currentState != BossState.Idle) return; 

        float firePointX = Mathf.Abs(firePoint.localPosition.x);
        if (player.position.x > transform.position.x)
        {
            spriteRenderer.flipX = false;
            firePoint.localPosition = new Vector2(firePointX, firePoint.localPosition.y);
            firePoint.localRotation = Quaternion.identity;
        }
        else
        {
            spriteRenderer.flipX = true;
            firePoint.localPosition = new Vector2(-firePointX, firePoint.localPosition.y);
            firePoint.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    // --- PHẦN NÃO BỘ: QUYẾT ĐỊNH CHIÊU THỨC ---
    // --- PHẦN NÃO BỘ: QUYẾT ĐỊNH CHIÊU THỨC (ĐÃ CẬP NHẬT) ---
    void DecideNextMove()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        // 1. Quá xa (> 10m): Boss tự đi bộ lại gần
        if (distance > shootRange) return;

        // 2. TẦM TRUNG (Từ 5m -> 10m)
        if (distance > dashRange)
        {
            // --- LOGIC KHI HÓA ĐIÊN (ENRAGED) ---
            if (isEnraged)
            {
                // Luôn bắn Shotgun thay vì đạn thường
                int rng = Random.Range(0, 100);
                if (rng < 70) 
                {
                    Debug.Log("Enraged: Xả Shotgun liên tục!");
                    StartCoroutine(ShotgunAttack()); 
                }
                else 
                {
                    // Vẫn giữ 30% lao vào đấm để gây áp lực
                    StartCoroutine(MeleeAttack()); 
                }
            }
            // --- LOGIC KHI BÌNH THƯỜNG ---
            else
            {
                // Tỉ lệ 1 viên cao hơn (70%)
                int rng = Random.Range(0, 100);
                if (rng < 70) 
                {
                    Debug.Log("Normal: Bắn tỉa 1 viên");
                    StartCoroutine(ShootArmProjectile()); 
                }
                else 
                {
                    Debug.Log("Normal: Bắn Shotgun (Hiếm)");
                    StartCoroutine(ShotgunAttack()); 
                }
            }
        }
        
        // 3. TẦM GẦN (< 5m)
        else 
        {
            // Ở gần thì Boss luôn ưu tiên Đấm, thi thoảng bắn Shotgun vào mặt
            // (Không thay đổi nhiều vì ở gần bắn 1 viên rất yếu)
            int rng = Random.Range(0, 100);
            if (rng < 70) 
            {
                StartCoroutine(MeleeAttack()); // 70% Đấm
            }
            else 
            {
                StartCoroutine(ShotgunAttack()); // 30% Shotgun vào mặt
            }
        }
    }

    // --- CÁC CHIÊU THỨC ---

    // 1. Hóa điên
    System.Collections.IEnumerator EnrageRoutine()
    {
        isEnraged = true;
        currentState = BossState.Idle;
        animator.SetTrigger("Enrage");
        actionCooldown = 1.0f; // Tăng tốc độ đánh

        Color enrageColor = new Color(1f, 0.7f, 0.7f);
        spriteRenderer.color = enrageColor;
        if (bossHealth != null) bossHealth.defaultColor = enrageColor;

        yield return new WaitForSeconds(1.5f);
        lastActionTime = Time.time;
    }

    // 2. Đấm (Melee) + Combo bắn bồi
    System.Collections.IEnumerator MeleeAttack()
    {
        currentState = BossState.MeleeAttack;
        lastActionTime = Time.time;

        // Lao tới
        Vector2 targetPos = player.position;
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
        float timer = 0;
        
        // Quay mặt theo hướng lao
        if (direction.x > 0) spriteRenderer.flipX = false; else spriteRenderer.flipX = true;

        while (timer < 0.5f && Vector2.Distance(transform.position, player.position) > 1.0f)
        {
            timer += Time.deltaTime;
            transform.position += (Vector3)direction * dashSpeed * Time.deltaTime;
            yield return null;
        }

        animator.SetTrigger("Melee");
        yield return new WaitForSeconds(1.0f); // Thời gian animation đấm

        // --- COMBO: Đấm xong mà Player vẫn lỳ ở gần thì BẮN BỒI ---
        float distAfterPunch = Vector2.Distance(transform.position, player.position);
        if (distAfterPunch < 4.0f)
        {
            Debug.Log("Combo: Đấm xong bắn bồi!");
            SpawnProjectile(0);   // Bắn 1 viên thẳng
            yield return new WaitForSeconds(0.3f);
        }

        currentState = BossState.Idle;
    }

    // 3. Bắn thường (Có tính toán đón đầu)
    System.Collections.IEnumerator ShootArmProjectile()
    {
        currentState = BossState.LaserAttack;
        lastActionTime = Time.time;
        
        // FacePlayer() ở đây bị bỏ qua để tránh xoay người khi đang bắn, 
        // nhưng nên xoay 1 lần trước khi bắn:
        if (player.position.x > transform.position.x) spriteRenderer.flipX = false;
        else spriteRenderer.flipX = true;

        animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(0.5f);

        if (projectilePrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            GolemProjectile script = bullet.GetComponent<GolemProjectile>();

            // Tính toán bắn đón đầu
            Vector2 targetPos = player.position;
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            
            // LƯU Ý: Nếu Unity báo lỗi linearVelocity, hãy đổi thành velocity
            if (playerRb != null)
            {
                float timeToHit = Vector2.Distance(transform.position, player.position) / 10f;
                // Nếu dùng Unity cũ (2022 trở xuống) đổi dòng dưới thành playerRb.velocity
                targetPos = (Vector2)player.position + (playerRb.linearVelocity * timeToHit * 0.5f); 
            }

            Vector2 dir = (targetPos - (Vector2)firePoint.position).normalized;
            script.Fire(dir);
        }
        
        yield return new WaitForSeconds(1f);
        currentState = BossState.Idle;
    }

    // 4. Bắn Shotgun (3 viên tỏa ra)
    System.Collections.IEnumerator ShotgunAttack()
    {
        currentState = BossState.LaserAttack;
        lastActionTime = Time.time;
        
        if (player.position.x > transform.position.x) spriteRenderer.flipX = false;
        else spriteRenderer.flipX = true;

        animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(0.5f);
        
        // Bắn 3 viên: -15 độ, 0 độ, +15 độ
        SpawnProjectile(0);
        SpawnProjectile(15);
        SpawnProjectile(-15);
        
        if (isEnraged)
    {
        SpawnProjectile(30);  // Trái rộng
        SpawnProjectile(-30); // Phải rộng
    }
        yield return new WaitForSeconds(1f);
        currentState = BossState.Idle;
    }

    // Hàm phụ trợ để sinh đạn
    void SpawnProjectile(float angleOffset)
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            GolemProjectile script = bullet.GetComponent<GolemProjectile>();
            
            Vector2 dir = (player.position - transform.position).normalized;
            // Xoay hướng bắn
            dir = Quaternion.Euler(0, 0, angleOffset) * dir;
            
            script.Fire(dir);
        }
    }

    public void OnBossDie()
    {
        StopAllCoroutines();
        if (currentLaser != null) Destroy(currentLaser);
        currentState = BossState.Dead;
    }
}
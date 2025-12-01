using UnityEngine;

public class MechaGolem_AI : MonoBehaviour
{
    public enum BossState { Idle, MeleeAttack, LaserAttack, Shield, Dead }
    public BossState currentState;

    [Header("Components")]
    public Transform player;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    [Header("Projectile Settings")]
    public GameObject projectilePrefab; // Prefab đạn tay
    public Transform firePoint;         // Vị trí bắn

    [Header("Laser Settings")]
    public GameObject laserPrefab;      // Prefab Laser

    [Header("Melee Settings")]
    public Transform meleePoint;        // Tâm cú đấm
    public float meleeRadius = 2f;      // Bán kính đấm
    public int meleeDamage = 20;        // Sát thương đấm

    [Header("Stats")]
    public float moveSpeed = 2f;
    public float dashSpeed = 15f;       // Tốc độ lướt nhanh
    public float meleeRange = 3f;       // Tầm gây dame
    public float dashRange = 6f;        // Tầm bắt đầu lướt (Để 6 cho chuẩn)
    public float shootRange = 10f;
    public float laserRange = 15f;

    [Header("Cooldowns")]
    public float actionCooldown = 2f;
    private float lastActionTime;

    [Header("Enrage Settings")]
    public bool isEnraged = false;
    private BossHealth bossHealth;

    // Biến quản lý tia Laser để xóa khi chết
    private GameObject currentLaser; 

    // --- HÀM CHECK MELEE (Gây dame) ---
    public void CheckMeleeHit()
    {
        // Quét va chạm (Tìm cả Player và Default)
        int targetLayer = LayerMask.GetMask("Default") | LayerMask.GetMask("Player");
        Collider2D[] hits = Physics2D.OverlapCircleAll(meleePoint.position, meleeRadius, targetLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player")) 
            {
                Debug.Log("BOSS ĐẤM TRÚNG PLAYER!");
                // Gây dame + Đẩy lùi
                hit.GetComponent<PlayerHealth>()?.TakeDamage(meleeDamage, transform);
            }
        }
    }

    // Vẽ vòng tròn đỏ để dễ chỉnh Melee Point
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
        
        bossHealth = GetComponent<BossHealth>();
        currentState = BossState.Idle;
    }

    void Update()
    {
        if (player == null || currentState == BossState.Dead) return;

        // Logic Nổi giận
        if (!isEnraged && bossHealth.currentHealth < (bossHealth.maxHealth * 0.5f))
        {
            StartCoroutine(EnrageRoutine());
            return;
        }

        FacePlayer();

        // Logic Di chuyển (Trôi)
        float distance = Vector2.Distance(transform.position, player.position);
        if (currentState == BossState.Idle && distance > dashRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }

        // Logic Tấn công
        if (Time.time > lastActionTime + actionCooldown && currentState == BossState.Idle)
        {
            DecideNextMove();
        }
    }

    System.Collections.IEnumerator EnrageRoutine()
    {
        isEnraged = true;
        currentState = BossState.Idle;

        Debug.Log("BOSS NỔI GIẬN!");
        animator.SetTrigger("Enrage");
        actionCooldown = 1.0f; // Hồi chiêu nhanh hơn

        Color enrageColor = new Color(1f, 0.7f, 0.7f);
        spriteRenderer.color = enrageColor;
        
        // Cập nhật màu gốc cho BossHealth
        if (bossHealth != null) bossHealth.defaultColor = enrageColor;

        yield return new WaitForSeconds(1.5f);
        lastActionTime = Time.time; 
    }

    void FacePlayer()
    {
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

    void DecideNextMove()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        // Nếu vào tầm lướt -> Lướt tới đấm
        if (distance <= dashRange)
        {
            StartCoroutine(MeleeAttack());
        }
        else
        {
            if (isEnraged)
            {
                int rng = Random.Range(0, 100);

                if (rng < 40) StartCoroutine(ShootArmProjectile()); // 40%
                else if (rng < 70) StartCoroutine(LaserBeamAttack()); // 30%
                else StartCoroutine(ShieldAbility()); // 30%
            }
            else
            {
                StartCoroutine(ShootArmProjectile());
            }
        }
    }

    System.Collections.IEnumerator MeleeAttack()
    {
        currentState = BossState.MeleeAttack;
        lastActionTime = Time.time;

        // Lướt tới
        Vector2 targetPos = player.position;
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
        float dashDuration = 0.5f;
        float timer = 0;

        while (timer < dashDuration && Vector2.Distance(transform.position, player.position) > 1.5f)
        {
            timer += Time.deltaTime;
            transform.position += (Vector3)direction * dashSpeed * Time.deltaTime;
            yield return null;
        }

        // Đấm
        animator.SetTrigger("Melee");
        yield return new WaitForSeconds(1.0f);
        
        currentState = BossState.Idle;
    }

    System.Collections.IEnumerator ShootArmProjectile()
    {
        currentState = BossState.LaserAttack;
        lastActionTime = Time.time;
        animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(0.5f);

        if (projectilePrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            GolemProjectile script = bullet.GetComponent<GolemProjectile>();
            Vector2 dir = (player.position - transform.position).normalized;
            script.Fire(dir);
        }

        yield return new WaitForSeconds(1f); 
        currentState = BossState.Idle;
    }

    System.Collections.IEnumerator LaserBeamAttack()
    {
        currentState = BossState.LaserAttack;
        lastActionTime = Time.time;
        Debug.Log("Gồng Laser...");
        animator.SetTrigger("Laser");
        yield return new WaitForSeconds(1.0f); 

        if (laserPrefab != null && firePoint != null)
        {
            currentLaser = Instantiate(laserPrefab, firePoint.position, Quaternion.identity);
            currentLaser.transform.parent = firePoint;
            Vector2 direction = (player.position - firePoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            currentLaser.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        yield return new WaitForSeconds(2.5f); 
        currentState = BossState.Idle;
    }

    System.Collections.IEnumerator ShieldAbility()
    {
        currentState = BossState.Shield;
        lastActionTime = Time.time;

        Debug.Log("Boss bật KHIÊN PHẢN ĐÒN!");
        animator.SetTrigger("Shield");
        
        spriteRenderer.color = Color.cyan; 
        bossHealth.isInvulnerable = true;

        // Chờ 3 giây (hoặc độ dài anim loop)
        yield return new WaitForSeconds(3.0f); 

        bossHealth.isInvulnerable = false;
        
        // Trả lại màu đúng (Đỏ nếu đang điên, Trắng nếu thường)
        if (isEnraged)
        {
            spriteRenderer.color = new Color(1f, 0.7f, 0.7f);
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
        
        // Cập nhật lại cho chắc
        if (bossHealth != null) bossHealth.defaultColor = spriteRenderer.color;

        currentState = BossState.Idle;
    }

    public void OnBossDie()
    {
        StopAllCoroutines();
        if (currentLaser != null) Destroy(currentLaser);
        currentState = BossState.Dead;
    }
}
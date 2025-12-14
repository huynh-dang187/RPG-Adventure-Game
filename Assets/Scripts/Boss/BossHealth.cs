using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BossHealth : MonoBehaviour
{
    [Header("Cài đặt Máu")]
    public int maxHealth = 500;
    public int currentHealth;

    [Header("Trạng thái")]
    public bool isInvulnerable = false;

    [Header("KÉO THẢ UI VÀO ĐÂY (Canvas riêng của Scene)")]
    public GameObject healthBarFrame; // Kéo cái khung (Frame)
    public Slider healthSlider;       // Kéo cái Slider
    public TextMeshProUGUI bossNameText; // Kéo cái Text tên

    [Header("Cổng thoát (Kéo thả)")]
    public GameObject exitPortal; 

    [Header("Hiệu ứng")]
    public Color flashColor = Color.red; 
    public float flashDuration = 0.1f;   
    private SpriteRenderer spriteRenderer;

    // --- SỬA LỖI TẠI ĐÂY: Đổi thành PUBLIC để AI Golem dùng được ---
    public Color defaultColor; 
    // --------------------------------------------------------------

    // AI References
    private Reaper_AI reaperAI;
    private MechaGolem_AI golemAI; 
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        
        reaperAI = GetComponent<Reaper_AI>();
        golemAI = GetComponent<MechaGolem_AI>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Lưu màu gốc
        if (spriteRenderer != null) defaultColor = spriteRenderer.color;

        // Setup Slider ban đầu
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        
        // MẶC ĐỊNH LÀ TẮT (Ẩn đi chờ bị đánh mới hiện)
        if (healthBarFrame != null) 
        {
            healthBarFrame.SetActive(false);
        }
    }

    public void TakeDamage(int damage, Transform hitTransform)
    {
        // 1. Check bất tử
        if (isInvulnerable)
        {
            if (hitTransform != null)
            {
                PlayerHealth player = hitTransform.GetComponent<PlayerHealth>();
                if (player != null) player.TakeDamage(1, transform);
            }
            return; 
        }

        // 2. Check chết
        if ((reaperAI != null && reaperAI.currentState == Reaper_AI.BossState.Dead) || 
            (golemAI != null && golemAI.currentState == MechaGolem_AI.BossState.Dead)) return;

        // 3. BẬT THANH MÁU (Nếu đang tắt)
        if (healthBarFrame != null && !healthBarFrame.activeSelf)
        {
            healthBarFrame.SetActive(true);
            
            // Set tên Boss
            if (bossNameText != null)
            {
                if (golemAI != null) bossNameText.text = "MECHA GOLEM";
                else if (reaperAI != null) bossNameText.text = "THE REAPER";
            }
        }

        // 4. Trừ máu
        currentHealth -= damage;
        if (healthSlider != null) healthSlider.value = currentHealth;

        // 5. Hiệu ứng
        if (spriteRenderer != null) StartCoroutine(FlashRoutine());

        // 6. Xử lý chết
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    IEnumerator FlashRoutine()
    {
        spriteRenderer.color = flashColor; 
        yield return new WaitForSeconds(flashDuration); 
        spriteRenderer.color = defaultColor; 
    }

    void Die()
    {
        if (healthBarFrame != null) healthBarFrame.SetActive(false); 
        if (exitPortal != null) exitPortal.SetActive(true); 
        if (animator != null) animator.SetTrigger("Die");
        
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        if (reaperAI != null) { reaperAI.currentState = Reaper_AI.BossState.Dead; reaperAI.enabled = false; }
        if (golemAI != null) { golemAI.OnBossDie(); golemAI.enabled = false; }

        Destroy(gameObject, 2f); 
    }
}
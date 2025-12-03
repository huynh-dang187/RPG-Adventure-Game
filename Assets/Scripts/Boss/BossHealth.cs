using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections; // Cần dòng này để dùng Coroutine

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 500;
    public int currentHealth;

    [Header("Status")]
    public bool isInvulnerable = false; // Biến để bật/tắt chế độ Bất tử

    [Header("UI Settings")]
    public Slider healthSlider;
    public GameObject healthBarFrame;
    public TextMeshProUGUI bossNameText;
    public GameObject exitPortal;

    [Header("Flash Effect")]
    public Color flashColor = Color.red; // Màu nháy khi bị đánh
    public float flashDuration = 0.1f;   // Thời gian nháy
    private SpriteRenderer spriteRenderer;
    
    // Đã đổi tên thành defaultColor và để public
    public Color defaultColor; 

    // --- KHAI BÁO CÁC LOẠI AI CỦA BOSS ---
    private Reaper_AI reaperAI;
    private MechaGolem_AI golemAI; 
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        
        // Tự động tìm component
        reaperAI = GetComponent<Reaper_AI>();
        golemAI = GetComponent<MechaGolem_AI>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Lưu màu gốc (dùng biến defaultColor)
        if (spriteRenderer != null) defaultColor = spriteRenderer.color;

        // Setup UI ban đầu
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        if (healthBarFrame != null) healthBarFrame.SetActive(false);
    }

    public void TakeDamage(int damage, Transform hitTransform)
    {
        // 1. XỬ LÝ KHIÊN PHẢN ĐÒN (Counter Shield)
        if (isInvulnerable)
        {
            if (hitTransform != null)
            {
                PlayerHealth player = hitTransform.GetComponent<PlayerHealth>();
                if (player != null)
                {
                    Debug.Log("PHẢN ĐÒN! Player đánh vào khiên!");
                    player.TakeDamage(1, transform);
                }
            }
            return; 
        }

        // 2. KIỂM TRA XEM BOSS ĐÃ CHẾT CHƯA
        if ((reaperAI != null && reaperAI.currentState == Reaper_AI.BossState.Dead) || 
            (golemAI != null && golemAI.currentState == MechaGolem_AI.BossState.Dead)) return;

        // 3. HIỆN UI
        if (healthBarFrame != null && !healthBarFrame.activeSelf)
        {
            healthBarFrame.SetActive(true);
            if (golemAI != null && bossNameText != null) bossNameText.text = "MECHA GOLEM";
            if (reaperAI != null && bossNameText != null) bossNameText.text = "THE REAPER";
        }

        // 4. TRỪ MÁU
        currentHealth -= damage;
        if (healthSlider != null) healthSlider.value = currentHealth;

        // 5. HIỆU ỨNG NHÁY ĐỎ
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashRoutine());
        }

        // 6. KIỂM TRA CHẾT
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    // Coroutine nháy đỏ
    IEnumerator FlashRoutine()
    {
        spriteRenderer.color = flashColor; // Chuyển sang đỏ
        yield return new WaitForSeconds(flashDuration); // Chờ xíu
        
        // SỬA LỖI Ở ĐÂY: Dùng defaultColor thay vì originalColor
        spriteRenderer.color = defaultColor; 
    }

    void Die()
    {
        Debug.Log("Boss đã chết!");
        
        if (healthBarFrame != null) healthBarFrame.SetActive(false);
        if (healthSlider != null) healthSlider.gameObject.SetActive(false);
        if (exitPortal != null) exitPortal.SetActive(true);
        if (animator != null) animator.SetTrigger("Die");

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        if (reaperAI != null)
        {
            reaperAI.currentState = Reaper_AI.BossState.Dead;
            reaperAI.enabled = false;
        }
        if (golemAI != null)
        {
            golemAI.OnBossDie(); 
            golemAI.enabled = false;
        }

        Destroy(gameObject, 2f); 
    }
}
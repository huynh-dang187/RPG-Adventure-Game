using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 500; // Golem máu trâu hơn chút
    public int currentHealth;

    [Header("UI Settings")]
    public Slider healthSlider;
    public GameObject healthBarFrame;
    public TextMeshProUGUI bossNameText;
    public GameObject exitPortal; // Cổng thoát (nếu có)

    // --- KHAI BÁO CÁC LOẠI AI CỦA BOSS ---
    private Reaper_AI reaperAI;
    private MechaGolem_AI golemAI; 
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        
        // Tự động tìm xem mình đang gắn trên con Boss nào
        reaperAI = GetComponent<Reaper_AI>();
        golemAI = GetComponent<MechaGolem_AI>();
        animator = GetComponent<Animator>();

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
        // 1. KIỂM TRA XEM BOSS ĐÃ CHẾT CHƯA (Tùy loại boss)
        if (reaperAI != null && reaperAI.currentState == Reaper_AI.BossState.Dead) return;
        if (golemAI != null && golemAI.currentState == MechaGolem_AI.BossState.Dead) return;

        // 2. HIỆN UI
        if (healthBarFrame != null && !healthBarFrame.activeSelf)
        {
            healthBarFrame.SetActive(true);
            // Đặt tên tùy boss (Optional)
            if (golemAI != null && bossNameText != null) bossNameText.text = "MECHA GOLEM";
        }

        // 3. TRỪ MÁU
        currentHealth -= damage;
        if (healthSlider != null) healthSlider.value = currentHealth;

        // Debug.Log("Boss mất " + damage + " máu. Còn: " + currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boss đã chết!");
        
        // Ẩn UI
        if (healthBarFrame != null) healthBarFrame.SetActive(false);
        if (healthSlider != null) healthSlider.gameObject.SetActive(false);

        // Hiện cổng (nếu có)
        if (exitPortal != null) exitPortal.SetActive(true);

        // Chạy Anim Chết
        if (animator != null) animator.SetTrigger("Die");

        // Tắt va chạm
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // TẮT AI (Tùy loại boss)
        if (reaperAI != null)
        {
            reaperAI.currentState = Reaper_AI.BossState.Dead;
            reaperAI.enabled = false;
        }
        if (golemAI != null)
        {
            golemAI.currentState = MechaGolem_AI.BossState.Dead;
            golemAI.enabled = false;
        }

        Destroy(gameObject, 5f); 
    }
}
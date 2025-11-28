using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHealth : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private float knockBackThrust = 15f;

    private int currentHealth;
    
    // --- KHAI BÁO CÁC SCRIPT RIÊNG CỦA SKELETON ---
    private SkeletonKnockback knockback; // Dùng script riêng
    private SkeletonFlash flash;         // Dùng script riêng
    private Animator animator; 
    private SkeletonAI skeletonAI;       // Dùng script riêng
    private PickUpSpawner pickUpSpawner; 

    private void Awake() {
        // Tự động tìm các script phụ trợ
        // Lưu ý: Flash và Animator tìm ở Children (con) cho chắc
        flash = GetComponentInChildren<SkeletonFlash>(); 
        knockback = GetComponent<SkeletonKnockback>();
        animator = GetComponentInChildren<Animator>(); 
        skeletonAI = GetComponent<SkeletonAI>();   
        pickUpSpawner = GetComponent<PickUpSpawner>();
    }

    private void Start() {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        
        // 1. Xử lý Đẩy lùi (SkeletonKnockback)
        if (knockback != null)
        {
            knockback.GetKnockedBack(PlayerController.Instance.transform, knockBackThrust);
        }
        
        // 2. Xử lý Nháy đèn (SkeletonFlash)
        if (flash != null)
        {
            StartCoroutine(flash.FlashRoutine());
        }
        
        // 3. Kiểm tra xem chết chưa
        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine() {
        // Đợi hiệu ứng nháy đèn xong mới xử lý chết (nếu có Flash)
        float waitTime = (flash != null) ? flash.GetRestoreMatTime() : 0f;
        yield return new WaitForSeconds(waitTime);
        DetectDeath();
    }

    public void DetectDeath() {
        if (currentHealth <= 0) {
            
            // --- BƯỚC 1: RƠI ĐỒ ---
            if (pickUpSpawner != null)
            {
                pickUpSpawner.DropItems();
            }

            // --- BƯỚC 2: HIỆU ỨNG NỔ ---
            if (deathVFXPrefab != null) 
            {
                Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            }

            // --- BƯỚC 3: XỬ LÝ CHẾT ---
            
            // A. Tắt não (SkeletonAI) ngay lập tức
            if (skeletonAI != null) 
            {
                skeletonAI.enabled = false; 
            }

            // B. Dừng trôi vật lý
            if (TryGetComponent(out Rigidbody2D rb))
            {
                rb.linearVelocity = Vector2.zero; // Unity 6
            }

            // C. Tắt va chạm
            if (TryGetComponent(out Collider2D col))
            {
                col.enabled = false;
            }

            // D. Chạy Animation Die
            if (animator != null)
            {
                animator.SetTrigger("die"); 
            }

            // E. Tắt script này luôn
            this.enabled = false; 
            EnemyManager manager = GetComponentInParent<EnemyManager>();
            if (manager != null)
            {
                manager.EnemyDefeated();
            }
            // F. Hủy xác sau 2 giây
            Destroy(gameObject, 2f);
        }
    }
}
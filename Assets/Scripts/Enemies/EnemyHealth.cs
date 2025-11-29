using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private GameObject deathVFXPrefab; 
    [SerializeField] private float knockBackThrust = 15f;

    private int currentHealth;
    private Knockback knockback;
    private Flash flash;
    private Animator animator; 
    private EnemyAI enemyAI;  

    private void Awake() {
        // Tìm Flash ở các object con luôn cho chắc
        flash = GetComponentInChildren<Flash>();
        if (flash == null) flash = GetComponent<Flash>();

        knockback = GetComponent<Knockback>();
        animator = GetComponent<Animator>(); 
        enemyAI = GetComponent<EnemyAI>();   
    }

    private void Start() {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        
        if (knockback != null)
        {
            knockback.GetKnockedBack(PlayerController.Instance.transform, knockBackThrust);
        }
        
        if (flash != null)
        {
            StartCoroutine(flash.FlashRoutine());
        }
        
        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine() {
        float waitTime = (flash != null) ? flash.GetRestoreMatTime() : 0f;
        yield return new WaitForSeconds(waitTime);
        DetectDeath();
    }

    public void DetectDeath() {
        if (currentHealth <= 0) {
            
            // 1. Rớt đồ
            if (TryGetComponent(out PickUpSpawner pickUpSpawner))
            {
                pickUpSpawner.DropItems();
            }

            // 2. Tạo hiệu ứng nổ (BÙM!)
            if (deathVFXPrefab != null) 
            {
                Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            }

            // 3. Báo cáo cho Manager
            EnemyManager manager = GetComponentInParent<EnemyManager>();
            if (manager != null)
            {
                manager.EnemyDefeated();
            }

            // 4. Xử lý xóa sổ quái vật
            // Tắt AI và Collider ngay lập tức để nó không đánh mình được nữa
            if (enemyAI != null) enemyAI.enabled = false;
            GetComponent<Collider2D>().enabled = false;

            // KIỂM TRA QUAN TRỌNG:
            // Nếu bạn muốn dùng Animation chết (ví dụ quái ngã từ từ)
            // thì hãy dùng try-catch hoặc kiểm tra kỹ Parameter.
            // Nhưng nếu muốn "Nổ bùm" rồi mất luôn thì xóa đoạn Animator đi hoặc sửa thành:
            
            Destroy(gameObject); // Xóa NGAY LẬP TỨC, không chờ 2s nữa
        }
    }
}
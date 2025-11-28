using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private GameObject deathVFXPrefab; // Biến này đang bị Null
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
            
            // Rớt đồ
            if (TryGetComponent(out PickUpSpawner pickUpSpawner))
            {
                pickUpSpawner.DropItems();
            }

            // --- ĐÂY LÀ ĐOẠN FIX LỖI ---
            // Kiểm tra: Nếu biến deathVFXPrefab CÓ dữ liệu thì mới tạo
            if (deathVFXPrefab != null) 
            {
                Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            }
            // ---------------------------

            if (animator != null)
            {
                animator.SetTrigger("die"); // Nhớ check tên trigger trong Animator là "die"

                if (enemyAI != null) enemyAI.enabled = false;
                GetComponent<Collider2D>().enabled = false;
                this.enabled = false; 
                Destroy(gameObject, 2f);
            }
            else 
            {
                Destroy(gameObject);
            }
        }
    }
}
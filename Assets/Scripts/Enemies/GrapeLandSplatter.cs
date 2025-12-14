using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeLandSplatter : MonoBehaviour
{
    // --- THÊM DÒNG NÀY ---
    [Header("Cài đặt Sát Thương")]
    [SerializeField] private int damageAmount = 10; // Chỉnh số này trong Unity
    // ---------------------

    private SpriteFade spriteFade;

    private void Awake() {
        spriteFade = GetComponent<SpriteFade>();
        SoundManager.Instance.PlaySound3D("Slime_Bullet_Drop", transform.position); 
    }

    private void Start() {
        StartCoroutine(spriteFade.SlowFadeRoutine());
        Invoke("DisableCollider", 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        
        // --- SỬA DÒNG NÀY ---
        // Thay số 1 bằng biến damageAmount
        playerHealth?.TakeDamage(damageAmount, transform); 
    }

    private void DisableCollider() {
        GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
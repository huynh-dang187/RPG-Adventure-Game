using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private GameObject deathVFXPrefab;

    private int currentHealth;
    private Knockback Knockback;
    private Flash flash ;

    private void Awake(){
        flash = GetComponent<Flash>();
        Knockback = GetComponent<Knockback>();
    }
    private void Start() {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        Knockback.GetKnockedBack(PlayerController.Instance.transform,15f);
        StartCoroutine(flash.FlashRoutine());
    }

    public void DetectDeath() {
        if (currentHealth <= 0) {
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

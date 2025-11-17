using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    private int damageAmount;

    private void Start() {
      
        MonoBehaviour currenActiveWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
        damageAmount = (currenActiveWeapon as IWeapon).GetWeaponInfo().weaponDamage;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        SoundManager.Instance.PlaySound3D("Hit", transform.position); 

        // 1. Kiểm tra Quái thường (Code GỐC của bạn)
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damageAmount); // (Code cũ của bạn)
            return; // Đã gây damage -> Xong
        }

        // 2. Kiểm tra Boss
        BossHealth bossHealth = other.gameObject.GetComponent<BossHealth>();
        if (bossHealth != null)
        {
            bossHealth.TakeDamage(damageAmount, PlayerController.Instance.transform);
            return; // Đã gây damage -> Xong
        }

        // 3. KIỂM TRA WISP (Đệ)
        WispHealth wispHealth = other.gameObject.GetComponent<WispHealth>();
        if (wispHealth != null)
        {
            wispHealth.TakeDamage(damageAmount, PlayerController.Instance.transform);
            return; // Đã gây damage -> Xong
        }
    }
}

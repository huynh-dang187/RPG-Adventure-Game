using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    private int damageAmount;

    private void Start() 
    {
        // Lấy damage từ vũ khí hiện tại
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            damageAmount = (ActiveWeapon.Instance.CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponDamage;
        }
        else
        {
            damageAmount = 1; // Damage mặc định
        }
    }
    
    // Code xử lý va chạm phải nằm trong hàm này
    private void OnTriggerEnter2D(Collider2D other) 
    {
        // 1. ƯU TIÊN KIỂM TRA SKELETON (Code mới thêm)
        // Dùng GetComponentInParent để tìm ở cả Cha lẫn Con
        SkeletonHealth skeleton = other.GetComponentInParent<SkeletonHealth>();
        if (skeleton != null)
        {
            skeleton.TakeDamage(damageAmount);
            SoundManager.Instance.PlaySound3D("Hit", transform.position);
            return; // Đã chém trúng thì dừng, không xét tiếp
        }

        // 2. Kiểm tra Quái thường (EnemyHealth)
        EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damageAmount);
            SoundManager.Instance.PlaySound3D("Hit", transform.position);
            return; 
        }

        // 3. Kiểm tra Boss
        BossHealth bossHealth = other.GetComponentInParent<BossHealth>();
        if (bossHealth != null)
        {
            bossHealth.TakeDamage(damageAmount, PlayerController.Instance.transform);
            SoundManager.Instance.PlaySound3D("Hit", transform.position);
            return; 
        }

        // 4. Kiểm tra Wisp (Đệ)
        WispHealth wispHealth = other.GetComponentInParent<WispHealth>();
        if (wispHealth != null)
        {
            wispHealth.TakeDamage(damageAmount, PlayerController.Instance.transform);
            SoundManager.Instance.PlaySound3D("Hit", transform.position);
            return; 
        }
    }
}
<<<<<<< HEAD
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;

    private WeaponInfo weaponInfo;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }
>>>>>>> main

    private void Update()
    {
        MoveProjectile();
<<<<<<< HEAD
    }

=======
        DetectFireDistance();
    }

    // Nhận thông tin vũ khí (damage, range,...)
    public void UpdateWeaponInfo(WeaponInfo weaponInfo)
    {
        this.weaponInfo = weaponInfo;
    }

    // Khi đạn va chạm
    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        Indestructible indestructible = other.GetComponent<Indestructible>();

        if (other.isTrigger) return;

        // ✅ Gây damage cho enemy
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(weaponInfo.weaponDamage);
            Debug.Log($"🔥 Projectile hit enemy: {other.name}, dealt {weaponInfo.weaponDamage} damage.");
        }

        // Nếu trúng vật thể không phá được
        if (indestructible != null)
        {
            Debug.Log($"💥 Projectile hit indestructible object: {other.name}");
        }

        // Spawn hiệu ứng va chạm
        if (particleOnHitPrefabVFX != null)
        {
            Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
        }

        // Hủy viên đạn
        Destroy(gameObject);
    }

    // Hủy đạn nếu bay quá xa
    private void DetectFireDistance()
    {
        if (weaponInfo == null) return;

        if (Vector3.Distance(transform.position, startPosition) > weaponInfo.weaponRange)
        {
            Destroy(gameObject);
        }
    }

    // Di chuyển viên đạn
>>>>>>> main
    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }
}

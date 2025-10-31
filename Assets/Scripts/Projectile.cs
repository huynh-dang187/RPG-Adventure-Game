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

    private void Update()
    {
        MoveProjectile();
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
    // Nếu là Player thì bỏ qua (tránh tự bắn chính mình)
    if (other.CompareTag("Player"))
        return;

    if (other.isTrigger) 
        return;

    EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
    Indestructible indestructible = other.GetComponent<Indestructible>();

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

    // ✅ Chỉ hủy đạn nếu trúng enemy hoặc vật cản
    if (enemyHealth != null || indestructible != null)
    {
        Destroy(gameObject);
    }
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
    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }
}

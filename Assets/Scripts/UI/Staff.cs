using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject magicLaser;
    [SerializeField] private Transform magicLaserSpawnPoint;

    // --- THÊM BIẾN NÀY ĐỂ CHỈNH KHOẢNG CÁCH ---
    [Header("Weapon Settings")]
    [Tooltip("Khoảng cách từ gậy đến người chơi. Tăng số này để gậy bay xa người hơn.")]
    [SerializeField] private float distanceFromPlayer = 0.8f; 
    // ------------------------------------------

    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;

    readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    private void Awake() {
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        MouseFollowWithOffset();
    }

    public void Attack() {
        myAnimator.SetTrigger(ATTACK_HASH);
    }

    public void SpawnStaffProjectileAnimEvent() {
        GameObject newLaser = Instantiate(magicLaser, magicLaserSpawnPoint.position, Quaternion.identity);
        newLaser.GetComponent<MagicLaser>().UpdateLaserRange(weaponInfo.weaponRange);
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);
        Vector2 direction = mousePos - playerScreenPoint;

        // 1. Tính góc xoay chuẩn
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Lấy transform của ActiveWeapon
        Transform weaponTransform = ActiveWeapon.Instance.transform;

        // 2. Xoay vũ khí theo chuột
        weaponTransform.rotation = Quaternion.Euler(0, 0, angle);

        // 3. XỬ LÝ LẬT HÌNH & VỊ TRÍ (QUAN TRỌNG)
        Vector3 scale = Vector3.one; 
        
        // Kiểm tra xem đang nhìn bên Trái hay Phải
        if (Mathf.Abs(angle) > 90)
        {
            // --- TRƯỜNG HỢP: NHÌN SANG TRÁI ---
            
            // A. Lật hình (Scale)
            scale.y = -1; 

            // B. Đẩy vị trí sang trái (Position) -> Dùng số Âm
            // Giữ nguyên Y và Z, chỉ thay đổi X
            weaponTransform.localPosition = new Vector3(-distanceFromPlayer, 0, 0);
        }
        else
        {
            // --- TRƯỜNG HỢP: NHÌN SANG PHẢI ---

            // A. Giữ nguyên hình
            scale.y = 1;

            // B. Đẩy vị trí sang phải (Position) -> Dùng số Dương
            weaponTransform.localPosition = new Vector3(distanceFromPlayer, 0, 0);
        }

        // Áp dụng scale
        weaponTransform.localScale = scale;

        // 4. XỬ LÝ LỚP HIỂN THỊ (TRƯỚC/SAU)
        if (mousePos.y < playerScreenPoint.y) 
        {
            spriteRenderer.sortingOrder = 10; // Trước mặt
        }
        else
        {
            spriteRenderer.sortingOrder = -1; // Sau lưng
        }
    }
}
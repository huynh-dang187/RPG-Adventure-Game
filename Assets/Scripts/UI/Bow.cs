using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;

    [Header("Tinh chỉnh Vị trí")]
    [Tooltip("Khoảng cách mặc định (khi quay phải).")]
    [SerializeField] private float defaultDistance = 0.5f; 

    [Tooltip("Khoảng cách CỘNG THÊM khi quay trái.")]
    [SerializeField] private float leftSideExtraDistance = 0.4f; 

    [Tooltip("Góc bù trừ (nếu cần).")]
    [SerializeField] private float rotationOffset = 0f;

    readonly int FIRE_HASH = Animator.StringToHash("Fire");
    private Animator myAnimator; 
    private SpriteRenderer spriteRenderer; 
    
    // Lưu kích thước gốc (ví dụ 0.7, 0.7, 1)
    private Vector3 originalScale;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        
        // --- FIX 1: Lấy trị tuyệt đối để luôn lưu số dương (tránh lỗi nếu lỡ chỉnh âm trong prefab) ---
        originalScale = new Vector3(
            Mathf.Abs(transform.localScale.x), 
            Mathf.Abs(transform.localScale.y), 
            Mathf.Abs(transform.localScale.z)
        );
    }

    // --- FIX 2: QUAN TRỌNG NHẤT - Xử lý ngay khi vừa đổi vũ khí ---
    private void OnEnable()
    {
        // Tính toán vị trí ngay lập tức, không chờ đến cuối khung hình
        MouseFollowWithLogic();
    }

    private void LateUpdate()
    {
        MouseFollowWithLogic();
    }

    public void Attack()
    {
        myAnimator.SetTrigger(FIRE_HASH);
        SoundManager.Instance.PlaySound3D("Arrow_Fire", transform.position);
        GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, ActiveWeapon.Instance.transform.rotation);
        newArrow.GetComponent<Projectile>().UpdateProjectileRange(weaponInfo.weaponRange);
    }

    public WeaponInfo GetWeaponInfo() { return weaponInfo; }

    private void MouseFollowWithLogic()
    {
        // 1. Tính Góc
        Vector3 pivotPos = ActiveWeapon.Instance.transform.position;
        // Kiểm tra camera null để tránh lỗi khi vừa start game
        if (Camera.main == null) return; 
        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; 

        Vector3 direction = mousePos - pivotPos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 2. Xoay Parent
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);

        // 3. XỬ LÝ KHOẢNG CÁCH (Fix lỗi dính người)
        float finalDistance = defaultDistance;

        // Nếu góc > 90 hoặc < -90 (đang quay trái)
        if (Mathf.Abs(angle) > 90) 
        {
            finalDistance += leftSideExtraDistance; 
        }

        // Áp dụng vị trí
        transform.localPosition = new Vector3(finalDistance, 0, 0);

        // 4. XỬ LÝ SCALE (Luôn dùng kích thước gốc dương)
        transform.localScale = originalScale;

        // 5. SORTING ORDER
        if (PlayerController.Instance != null) // Check null cho chắc
        {
            if (mousePos.y > PlayerController.Instance.transform.position.y + 0.5f) 
            {
                spriteRenderer.sortingOrder = -1; 
            }
            else 
            {
                spriteRenderer.sortingOrder = 10; 
            }
        }
    }
}
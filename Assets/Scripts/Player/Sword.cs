using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    // ... (Giữ nguyên các biến cũ) ...
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private float swordAttackCD = .5f;
    [SerializeField] private WeaponInfo weaponInfo;

    // --- THÊM BIẾN NÀY ĐỂ CHỈNH HƯỚNG KIẾM ---
    [Header("Weapon Settings")]
    [Tooltip("Chỉnh số này để kiếm xoay đúng hướng (thử 90 hoặc -90)")]
    [SerializeField] private float rotationOffset = 0f; 
    // ------------------------------------------

    private Transform weaponCollider;
    private Animator myAnimator;
    private GameObject slashAnim;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        weaponCollider = PlayerController.Instance.GetWeaponCollider();
        slashAnimSpawnPoint = GameObject.Find("SlashSpawnPoint").transform;
    }

    private void OnEnable() {
        if (weaponCollider == null) weaponCollider = PlayerController.Instance.GetWeaponCollider();
        if (slashAnimSpawnPoint == null) slashAnimSpawnPoint = GameObject.Find("SlashSpawnPoint").transform;
        if (myAnimator != null) myAnimator.Rebind();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    // ... (Giữ nguyên các hàm Attack, GetWeaponInfo, AnimEvent...) ...
    public WeaponInfo GetWeaponInfo() { return weaponInfo; }
    
    public void Attack()
    {
        myAnimator.SetTrigger("Attack");
        weaponCollider.gameObject.SetActive(true);
        slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
        slashAnim.transform.parent = this.transform.parent;
        SoundManager.Instance.PlaySound3D("Slash", transform.position); 
    }

    public void DoneAttackingAnimEvent() { weaponCollider.gameObject.SetActive(false); }

    public void SwingUpFlipAnimEvent() {
        if (slashAnim == null) return;
        slashAnim.transform.rotation = Quaternion.Euler(-180, 0, 0);
        if (PlayerController.Instance.FacingLeft) slashAnim.GetComponent<SpriteRenderer>().flipX = true;
    }

    public void SwingDownFlipAnimEvent() {
        if (slashAnim == null) return;
        slashAnim.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (PlayerController.Instance.FacingLeft) slashAnim.GetComponent<SpriteRenderer>().flipX = true;
    }

    // --- HÀM ĐÃ ĐƯỢC CHỈNH SỬA LOGIC ---
    private void MouseFollowWithOffset() {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        // 1. SỬA LỖI TÍNH GÓC: Phải trừ đi vị trí Player để lấy hướng Vector
        Vector3 direction = mousePos - playerScreenPoint;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Lấy transform của ActiveWeapon
        Transform weaponTransform = ActiveWeapon.Instance.transform;

        // 2. XOAY VŨ KHÍ
        // Luôn xoay theo góc chuột + Offset để chỉnh dáng kiếm
        weaponTransform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);
        weaponCollider.transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset); // Xoay cả collider theo

        // 3. XỬ LÝ LẬT TRÁI PHẢI (FLIP)
        // Dùng LocalScale.Y để lật hình sẽ chuẩn hơn là xoay trục Y 180 độ
        if (mousePos.x < playerScreenPoint.x) {
            // A. CHUỘT BÊN TRÁI
            // Lật ngược hình lại để kiếm không bị lộn ngược đầu
            weaponTransform.localScale = new Vector3(1, -1, 1); 
            
            // Đưa vũ khí sang bên trái người chơi (Position X âm)
            float currentX = Mathf.Abs(weaponTransform.localPosition.x); 
            weaponTransform.localPosition = new Vector3(-currentX, weaponTransform.localPosition.y, weaponTransform.localPosition.z);
        } 
        else {
            // B. CHUỘT BÊN PHẢI
            // Bình thường
            weaponTransform.localScale = new Vector3(1, 1, 1);

            // Đưa vũ khí sang phải người chơi (Position X dương)
            float currentX = Mathf.Abs(weaponTransform.localPosition.x);
            weaponTransform.localPosition = new Vector3(currentX, weaponTransform.localPosition.y, weaponTransform.localPosition.z);
        }

        // 4. SORTING ORDER (Giữ nguyên logic cũ của bạn)
        if (mousePos.y < playerScreenPoint.y) 
        {
            spriteRenderer.sortingOrder = 10; 
        }
        else
        {
            spriteRenderer.sortingOrder = -1;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }

    private PlayerControls playerControls;
    private bool attackButtonDown;
    private bool isAttacking;

    // 🟢 Gọi base Awake() và khởi tạo input
    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        // Đảm bảo input luôn hoạt động
        if (playerControls == null)
            playerControls = new PlayerControls();

        playerControls.Enable();

        // Đăng ký event chỉ 1 lần
        playerControls.Combat.Attack.started -= OnAttackStarted;
        playerControls.Combat.Attack.canceled -= OnAttackCanceled;
        playerControls.Combat.Attack.started += OnAttackStarted;
        playerControls.Combat.Attack.canceled += OnAttackCanceled;
    }

    private void OnDisable()
    {
        if (playerControls != null)
            playerControls.Disable();
    }

    private void Update()
    {
        Attack();
    }

    // 🟢 Khi đổi vũ khí
    public void NewWeapon(MonoBehaviour weaponScript)
    {
        // Ẩn vũ khí hiện tại (nếu có)
        if (CurrentActiveWeapon != null)
        {
            CurrentActiveWeapon.gameObject.SetActive(false);
        }

        // Gán vũ khí mới
        CurrentActiveWeapon = weaponScript;

        if (CurrentActiveWeapon != null)
        {
            CurrentActiveWeapon.gameObject.SetActive(true);

            // Reset Animator nếu có
            var animator = CurrentActiveWeapon.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Rebind();
                animator.Update(0f);
            }

            // 🧩 Reset trạng thái attack khi đổi vũ khí
            isAttacking = false;
            attackButtonDown = false;

            Debug.Log($"Trang bị vũ khí mới: {weaponScript.name}");
        }
        else
        {
            Debug.LogWarning("⚠️ CurrentActiveWeapon bị null sau khi trang bị!");
        }
    }

    // 🧩 Đặt null khi bỏ hết vũ khí
    public void WeaponNull()
    {
        CurrentActiveWeapon = null;
        isAttacking = false;
        attackButtonDown = false;
    }

    // 🧩 Hàm tấn công chính
    private void Attack()
    {
        if (attackButtonDown && !isAttacking && CurrentActiveWeapon != null)
        {
            isAttacking = true;

            Debug.Log($"Tấn công bằng: {CurrentActiveWeapon.name} (activeSelf={CurrentActiveWeapon.gameObject.activeSelf})");

            (CurrentActiveWeapon as IWeapon)?.Attack();
        }
    }

    private void OnAttackStarted(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        attackButtonDown = true;
    }

    private void OnAttackCanceled(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        attackButtonDown = false;
    }

    public void ToggleIsAttacking(bool value)
    {
        isAttacking = value;
    }
}

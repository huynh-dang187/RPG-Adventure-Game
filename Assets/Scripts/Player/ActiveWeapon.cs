using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }

    private PlayerControls playerControls;
    private bool attackButtonDown;
    private bool isAttacking;

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();
    }

    private void Update()
    {
        Attack();
    }

    // 🟢 Khi gọi từ ActiveInventory hoặc hệ thống đổi vũ khí
    public void NewWeapon(MonoBehaviour newWeapon)
    {
        // Reset trạng thái tấn công trước khi gán vũ khí mới
        isAttacking = false;
        attackButtonDown = false;

        CurrentActiveWeapon = newWeapon;
    }

    public void WeaponNull()
    {
        // Khi bỏ vũ khí (ví dụ đổi sang slot trống)
        isAttacking = false;
        attackButtonDown = false;

        CurrentActiveWeapon = null;
    }

    public void ToggleIsAttacking(bool value)
    {
        isAttacking = value;
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private void Attack()
    {
        if (attackButtonDown && !isAttacking && CurrentActiveWeapon != null)
        {
            isAttacking = true;
            (CurrentActiveWeapon as IWeapon)?.Attack();
        }
    }
}

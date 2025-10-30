using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
<<<<<<< HEAD

    [SerializeField] private WeaponInfo weaponInfo;
    private void Update()
    {
        MouseFollowWithOffset();
    }
    public void Attack()
    {
        Debug.Log("Staff Attack");
=======
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject magicLaser;
    [SerializeField] private Transform magicLaserSpawnPoint;

    private Animator myAnimator;

    readonly int AttackHash = Animator.StringToHash("Attack");

    private void Awake() {
        myAnimator = GetComponent<Animator>();
    }

    private void Update() {
        MouseFollowWithOffset();
    }


    public void Attack() {
        myAnimator.SetTrigger(AttackHash);
    }

    public void SpawnStaffProjectileAnimEvent() {
        GameObject newLaser = Instantiate(magicLaser, magicLaserSpawnPoint.position, Quaternion.identity);
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
>>>>>>> main
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
<<<<<<< HEAD
        if (mousePos.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);

=======

        if (mousePos.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
>>>>>>> main
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
<<<<<<< HEAD
    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
=======
>>>>>>> main
}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Staff : MonoBehaviour , IWeapon
{
    public void Attack()
    {
        Debug.Log("Staff Attack");
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
    
}

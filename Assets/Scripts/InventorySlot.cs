using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponInfo;

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}

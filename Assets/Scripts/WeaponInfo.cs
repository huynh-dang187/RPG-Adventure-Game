using UnityEngine;
using System.Collections.Generic;
using System.Collections;


[CreateAssetMenu(menuName = "New Weapon")]
public class WeaponInfo : ScriptableObject
{
    public GameObject weaponPrefab;
    public float weaponCooldown;

}

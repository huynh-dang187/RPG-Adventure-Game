using UnityEngine;
using System.Collections.Generic;
using System.Collections;



public class Shooter : MonoBehaviour,IEnemy 
{
    [SerializeField] private GameObject bulletPrefab;

    public void Attack()
    {
        Vector3 targetDirection = PlayerController.Instance.transform.position - transform.position;

        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        newBullet.transform.right = targetDirection;

    }
}

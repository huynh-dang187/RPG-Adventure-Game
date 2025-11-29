using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestPickUpSpawner : MonoBehaviour
{
    [Header("Item Prefabs")]
    [SerializeField] private GameObject goldCoin, healthGlobe, staminaGlobe;

    [Header("Settings")]
    [SerializeField] private float explosionForce = 300f; // Lực bắn

    public void DropItems() 
    {
        // Logic Random cũ của bạn
        int randomNum = Random.Range(1, 4); // Random 1, 2, 3 (đã sửa thành 4 để chắc chắn rơi đồ)

        if (randomNum == 1)
        {
            LaunchItem(healthGlobe);
        }
        else if (randomNum == 2)
        {
            LaunchItem(staminaGlobe);
        }
        else if (randomNum == 3)
        {
            int randomAmountOfGold = Random.Range(1, 4);
            for(int i = 0; i < randomAmountOfGold; i++)
            {
                LaunchItem(goldCoin);
            }
        }

        // Âm thanh (nếu bạn đã có SoundManager thì giữ nguyên)
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound3D("Coin_Drop", transform.position); 
    }

    // Hàm mới: Tạo vật phẩm và bắn nó đi
    private void LaunchItem(GameObject itemPrefab)
    {
        if (itemPrefab == null) return;

        // 1. Tạo ra vật phẩm tại vị trí rương
        GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity);

        // 2. Lấy Rigidbody và thêm lực
        Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Tạo hướng ngẫu nhiên tròn 360 độ
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            rb.AddForce(randomDirection * explosionForce);
        }
    }
}
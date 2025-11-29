using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
    private Animator animator;
    
    // ĐÃ SỬA: Đổi tên biến cho khớp với script thực tế của bạn
    private ChestPickUpSpawner spawner; 
    
    private bool isOpened = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        
        // ĐÃ SỬA: Tìm đúng component ChestPickUpSpawner
        spawner = GetComponent<ChestPickUpSpawner>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOpened)
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        isOpened = true;

        // 1. Chạy Animation
        if (animator != null)
        {
            animator.SetTrigger("Open");
        }

        // 2. Rớt đồ
        if (spawner != null)
        {
            spawner.DropItems();
        }
        else
        {
            // Nếu vẫn lỗi thì dòng này sẽ hiện ra để báo
            Debug.LogError("Vẫn chưa tìm thấy script ChestPickUpSpawner! Kiểm tra lại Inspector.");
        }
    }
}
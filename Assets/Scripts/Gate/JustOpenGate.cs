using UnityEngine;

public class JustOpenGate : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite closedSprite; 
    public Sprite openSprite;   

    [Header("Components")]
    public SpriteRenderer spriteRenderer; 
    public Collider2D solidCollider; 

    // Biến mới: Dùng để ghi nhớ trạng thái khóa cửa
    private bool isLocked = false; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra: Nếu cửa đã bị khóa rồi thì không làm gì cả, thoát luôn
        if (isLocked) return;

        if (other.CompareTag("Player"))
        {
            OpenGate();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Kiểm tra: Nếu đã khóa rồi thì thôi
        if (isLocked) return;

        if (other.CompareTag("Player"))
        {
            CloseGate();
            
            // --- THAY ĐỔI QUAN TRỌNG Ở ĐÂY ---
            // Sau khi đóng cửa xong, ta bật cờ khóa lên
            isLocked = true; 
            // Từ giờ trở đi, hàm OnTriggerEnter2D sẽ bị chặn lại bởi dòng if(isLocked) ở trên
        }
    }

    void OpenGate()
    {
        spriteRenderer.sprite = openSprite;
        if (solidCollider != null) solidCollider.enabled = false;
        
        // SoundManager.Instance.PlaySound3D("Gate_Open", transform.position);
    }

    void CloseGate()
    {
        spriteRenderer.sprite = closedSprite;
        if (solidCollider != null) solidCollider.enabled = true;

        // SoundManager.Instance.PlaySound3D("Gate_Close", transform.position);
    }
}
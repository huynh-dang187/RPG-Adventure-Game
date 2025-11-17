using UnityEngine;

public class Wisp_AI : MonoBehaviour
{
    public float moveSpeed = 2f;    // Tốc độ con ma
    public int damage = 5;          // Sát thương con ma
    private Transform player;
    
    private bool isSpawned = false; // THÊM BIẾN NÀY (Cái phanh)

    void Start()
    {
        // Tìm Player bằng Tag (Tag "Player" phải gán cho Player)
        try
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        catch (System.Exception)
        {
            Debug.LogError("MA TRƠI KHÔNG TÌM THẤY PLAYER!");
            Destroy(gameObject); // Tự hủy nếu không tìm thấy
        }
    }

    void Update()
{
    // Chỉ di chuyển NẾU Player tồn tại VÀ đã spawn xong
    if (player != null && isSpawned)
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }
}

    // Xử lý va chạm (vì đã tick Is Trigger)
    void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem có va chạm với "Player" không
        if (other.CompareTag("Player"))
        {
            // Gây sát thương
            Debug.Log("Ma trơi chạm trúng Player!");
            other.GetComponent<PlayerHealth>().TakeDamage(damage, transform);
            
            // Tự hủy sau khi chạm
            Destroy(gameObject); 
        }
    }

    // Thêm hàm này vào BẤT CỨ ĐÂU bên trong script
    public void OnSpawnAnimationComplete()
    {
        isSpawned = true; // "Nhả phanh"
    }
}
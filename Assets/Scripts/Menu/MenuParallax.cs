using UnityEngine;

public class MenuParallax : MonoBehaviour
{
    [Header("Càng cao thì di chuyển càng mạnh")]
    public float offsetMultiplier = 20f; // Biên độ di chuyển
    public float smoothTime = 0.3f;      // Độ mượt (càng cao càng chậm)

    private Vector2 startPosition;
    private Vector3 velocity;

    void Start()
    {
        // Lưu lại vị trí ban đầu của ảnh
        startPosition = transform.position;
    }

    void Update()
    {
        // Lấy vị trí chuột trên màn hình (từ 0 đến 1)
        Vector2 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        
        // Chuyển đổi để tâm màn hình là (0,0) thay vì góc dưới trái
        float offsetX = (mousePos.x - 0.5f) * offsetMultiplier;
        float offsetY = (mousePos.y - 0.5f) * offsetMultiplier;

        // Tính vị trí mục tiêu
        Vector3 targetPos = new Vector3(
            startPosition.x + offsetX, // Di chuyển ngược chiều chuột thì dùng dấu -
            startPosition.y + offsetY,
            transform.position.z
        );

        // Di chuyển mượt mà tới vị trí mục tiêu
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}
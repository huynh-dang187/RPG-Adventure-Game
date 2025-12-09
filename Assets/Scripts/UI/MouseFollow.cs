using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    // Khai báo biến để chứa cái ảnh cây cung
    private SpriteRenderer weaponRenderer;

    private void Update()
    {
        // Tự động tìm SpriteRenderer trong các con (Bow Clone) nếu chưa có
        if (weaponRenderer == null)
        {
            weaponRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        FaceMouse();
    }

    private void FaceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // 1. Tính hướng và góc xoay
        Vector2 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 2. Xoay ActiveWeapon
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // 3. XỬ LÝ LẬT HÌNH (Trái/Phải)
        Vector3 scale = Vector3.one; // Mặc định là (1,1,1)
        if (Mathf.Abs(angle) > 90)
        {
            scale.y = -1; // Lật ngược nếu nhìn sang trái
        }
        transform.localScale = scale;

        // 4. XỬ LÝ TRƯỚC SAU (Trên/Dưới) - QUAN TRỌNG
        // Nếu đã tìm thấy ảnh cây cung
        if (weaponRenderer != null)
        {
            // Nếu chuột nằm cao hơn nhân vật (Aim lên trên) -> Đưa cung ra sau lưng
            if (direction.y > 0)
            {
                weaponRenderer.sortingOrder = -1; // Số nhỏ hơn Order của Player
            }
            // Nếu chuột nằm thấp hơn nhân vật (Aim xuống dưới) -> Đưa cung ra trước mặt
            else
            {
                weaponRenderer.sortingOrder = 10; // Số lớn hơn Order của Player
            }
        }
    }
}
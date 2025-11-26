using UnityEngine;

public class StairsLayerTrigger : MonoBehaviour
{
    [Header("Cài đặt Tầng Trên")]
    public string layerUpper = "Player_High";       
    public string sortingLayerUpper = "Floor_High"; 

    [Header("Cài đặt Tầng Dưới")]
    public string layerLower = "Default";       
    public string sortingLayerLower = "Default"; 

    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // 1. Xử lý khi ĐANG ĐI trong cầu thang
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // So sánh với TÂM CỦA CÁI HỘP (chính xác hơn)
            if (other.transform.position.y > boxCollider.bounds.center.y)
            {
                SetLayer(other.gameObject, layerUpper, sortingLayerUpper);
            }
            else
            {
                SetLayer(other.gameObject, layerLower, sortingLayerLower);
            }
        }
    }

    // 2. Xử lý khi BƯỚC RA KHỎI cầu thang
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Nếu bước ra mà chân đang ở CAO HƠN tâm hộp -> Giữ ở Tầng Trên
            if (other.transform.position.y > boxCollider.bounds.center.y)
            {
                SetLayer(other.gameObject, layerUpper, sortingLayerUpper);
            }
            // Nếu bước ra mà chân đang ở THẤP HƠN tâm hộp -> Về Tầng Dưới
            else
            {
                SetLayer(other.gameObject, layerLower, sortingLayerLower);
            }
        }
    }

    private void SetLayer(GameObject target, string layerName, string sortingLayerName)
    {
        int layerID = LayerMask.NameToLayer(layerName);
        if (layerID != -1) 
        {
            if (target.layer != layerID) 
            {
                target.layer = layerID;
                // Debug.Log("Đổi Layer vật lý: " + layerName); // Bật lên nếu cần test
            }
        }

        SpriteRenderer sr = target.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            if (sr.sortingLayerName != sortingLayerName) 
            {
                sr.sortingLayerName = sortingLayerName;
                // Debug.Log("Đổi Sorting Layer: " + sortingLayerName); // Bật lên nếu cần test
            }
        }
    }
}
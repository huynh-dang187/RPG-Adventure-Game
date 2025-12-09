// Tạo file script mới tên SelfDestruct.cs và gắn vào SpawnVFX
using UnityEngine;
public class SelfDestruct : MonoBehaviour
{
    // Thời gian tồn tại của hiệu ứng (chỉnh cho khớp với độ dài animation)
    public float lifetime = 1.5f; 
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
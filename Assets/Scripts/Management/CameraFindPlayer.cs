using UnityEngine;
using Cinemachine; // Bắt buộc phải có dòng này

public class CameraFindPlayer : MonoBehaviour
{
    void Start()
    {
        // 1. Lấy component Camera ảo trên chính object này
        CinemachineVirtualCamera vcam = GetComponent<CinemachineVirtualCamera>();

        // 2. Tìm Player trong game (Dựa vào Tag "Player" hoặc Script PlayerController)
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // 3. Gán Player vào ô Follow
        if (player != null && vcam != null)
        {
            vcam.Follow = player.transform;
            Debug.Log("Camera đã tìm thấy và Follow Player!");
        }
        else
        {
            Debug.LogError("Lỗi: Không tìm thấy Player hoặc Virtual Camera!");
        }
    }
}
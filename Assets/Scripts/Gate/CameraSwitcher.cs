using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera targetCam;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra Player chuẩn
        if (other.CompareTag("Player") || (other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Player")))
        {
            Debug.Log("--> Kích hoạt Camera: " + targetCam.name);

            var allCameras = FindObjectsOfType<CinemachineVirtualCamera>();
            foreach (var cam in allCameras) cam.Priority = 0;

            if (targetCam != null) 
            {
                targetCam.Priority = 10;
                
                // --- DÒNG QUAN TRỌNG CẦN THÊM ---
                // Tự động bắt Camera phải đi theo Player vừa chạm vào trigger
                targetCam.Follow = other.transform; 
                // --------------------------------
            }
        }
    }
}
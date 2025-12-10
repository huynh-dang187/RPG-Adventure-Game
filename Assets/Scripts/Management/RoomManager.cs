using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoomManager : MonoBehaviour
{
    [Header("1. Cài đặt Cổng ra")]
    [SerializeField] private GameObject exitDoor;

    [Header("2. Điểm Zoom (BẮT BUỘC)")]
    [SerializeField] private Transform zoomTarget;

    [Header("3. Camera (Kéo 'Virtual Camera' vào đây)")]
    [SerializeField] private CinemachineVirtualCamera targetCamera; // Kéo cái Virtual Camera vào đây

    [Header("4. Quản lý Quái vật")]
    [SerializeField] private Transform enemyGroup;

    [Header("5. Thông số Zoom")]
    [SerializeField] private float zoomSize = 3f;
    [SerializeField] private float zoomSpeed = 1.5f;
    [SerializeField] private float waitTime = 2.5f;

    private List<GameObject> enemies = new List<GameObject>();
    private bool isDoorOpened = false;

    private void Start()
    {
        if (exitDoor != null) exitDoor.SetActive(false);

        if (enemyGroup != null)
        {
            foreach (Transform child in enemyGroup)
                if (child.gameObject.activeSelf) enemies.Add(child.gameObject);
        }
    }

    private void Update()
    {
        if (!isDoorOpened) CheckEnemies();
        
        // Cheat K: Diệt quái
        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (var enemy in enemies) if (enemy != null) Destroy(enemy);
            enemies.Clear(); // Xóa list ngay để kích hoạt luôn
        }
    }

    private void CheckEnemies()
    {
        // Dọn dẹp list
        for (int i = enemies.Count - 1; i >= 0; i--)
            if (enemies[i] == null) enemies.RemoveAt(i);

        if (enemies.Count == 0)
        {
            isDoorOpened = true;
            StartCoroutine(OpenDoorSequence());
        }
    }

    private IEnumerator OpenDoorSequence()
    {
        if (exitDoor != null) exitDoor.SetActive(true);

        if (CameraController.Instance != null && zoomTarget != null)
        {
            // --- [PHẦN SỬA ĐỔI QUAN TRỌNG] ---
            // Dùng Behaviour để tắt được cả Confiner cũ lẫn Confiner 2D mới
            Behaviour confiner = null;

            if (targetCamera != null) {
                // Thử tìm Confiner bản cũ
                confiner = targetCamera.GetComponent<CinemachineConfiner>();
                
                // Nếu không thấy, thử tìm Confiner 2D (bản Unity 6)
                if (confiner == null) {
                    // Tìm theo tên để tránh lỗi biên dịch nếu thiếu thư viện
                    confiner = targetCamera.GetComponent("CinemachineConfiner2D") as Behaviour;
                }
            }

            // Tắt Confiner (Mở khóa nhà tù)
            if (confiner != null) {
                confiner.enabled = false;
                Debug.Log("🔓 Đã tắt Confiner thành công!");
            }
            // ----------------------------------

            CameraController.Instance.SetCameraTarget(zoomTarget);
            CameraController.Instance.ZoomTo(zoomSize, zoomSpeed);

            yield return new WaitForSeconds(waitTime);

            CameraController.Instance.SetPlayerCameraFollow();
            CameraController.Instance.ResetZoom(zoomSpeed);

            yield return new WaitForSeconds(zoomSpeed);
            
            // Bật lại Confiner (Nhốt lại)
            if (confiner != null) confiner.enabled = true;
        }
    }
}
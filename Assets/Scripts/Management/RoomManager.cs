using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoomManager : MonoBehaviour
{
    // [MỚI] Thêm biến này để quản lý Camera an toàn hơn
    public CameraController camControl; 

    [Header("1. Cài đặt Cổng ra")]
    [SerializeField] private GameObject exitDoor;

    [Header("2. Điểm Zoom (BẮT BUỘC)")]
    [SerializeField] private Transform zoomTarget;

    [Header("3. Camera (Kéo 'Virtual Camera' vào đây)")]
    [SerializeField] private CinemachineVirtualCamera targetCamera; 

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

        // Tự tìm CameraController nếu chưa kéo
        if (camControl == null) camControl = Object.FindFirstObjectByType<CameraController>();

        if (enemyGroup != null)
        {
            foreach (Transform child in enemyGroup)
                if (child.gameObject.activeSelf) enemies.Add(child.gameObject);
        }
    }

    private void Update()
    {
        if (!isDoorOpened) CheckEnemies();
        
        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (var enemy in enemies) if (enemy != null) Destroy(enemy);
            enemies.Clear(); 
        }
    }

    private void CheckEnemies()
    {
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

        if (camControl != null && zoomTarget != null)
        {
            Behaviour confiner = null;

            if (targetCamera != null) {
                confiner = targetCamera.GetComponent<CinemachineConfiner>();
                if (confiner == null) {
                    confiner = targetCamera.GetComponent("CinemachineConfiner2D") as Behaviour;
                }
            }

            if (confiner != null) confiner.enabled = false;

            camControl.SetCameraTarget(zoomTarget);
            camControl.ZoomTo(zoomSize, zoomSpeed);

            yield return new WaitForSeconds(waitTime);

            camControl.SetPlayerCameraFollow();
            camControl.ResetZoom(zoomSpeed);

            yield return new WaitForSeconds(zoomSpeed);
            
            if (confiner != null) confiner.enabled = true;
        }
    }
}
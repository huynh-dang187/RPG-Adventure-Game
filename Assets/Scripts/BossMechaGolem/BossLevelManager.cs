using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossLevelManager : MonoBehaviour
{
    [Header("1. KẾT NỐI HỆ THỐNG")]
    public CameraController camControl; 

    [Tooltip("Kéo VCam_Room2 vào đây")]
    public CinemachineVirtualCamera virtualCam; 

    [Header("2. MỤC TIÊU (BOSS)")]
    [SerializeField] private GameObject bossObject; 

    [Header("3. Cài đặt Cổng & Zoom")]
    [SerializeField] private GameObject exitDoor;
    [SerializeField] private Transform zoomTarget; 

    [Header("4. Thông số Zoom")]
    [SerializeField] private float zoomSize = 2f; 
    [SerializeField] private float zoomSpeed = 1.5f;
    [SerializeField] private float waitTime = 3f;

    private bool isCutscenePlayed = false;

    private void Start()
    {
        if (exitDoor != null) exitDoor.SetActive(false);

        // Tự tìm CameraController nếu chưa kéo
        if (camControl == null) camControl = Object.FindFirstObjectByType<CameraController>();

        // --- [THÊM MỚI QUAN TRỌNG] ---
        // Ngay khi vào màn Boss, ép CameraController nhận diện VCam_Room2
        // VÀ ép VCam_Room2 bám theo Player ngay lập tức
        if (camControl != null && virtualCam != null) {
            camControl.SetActiveCamera(virtualCam);
        }
        // -----------------------------
    }

    private void Update()
    {
        if (!isCutscenePlayed && bossObject == null)
        {
            isCutscenePlayed = true;
            StartCoroutine(OpenDoorSequence());
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (bossObject != null) Destroy(bossObject);
        }
    }

    private IEnumerator OpenDoorSequence()
    {
        if (exitDoor != null) exitDoor.SetActive(true);

        if (camControl != null && zoomTarget != null && virtualCam != null)
        {
            // Đảm bảo lại lần nữa trước khi zoom
            camControl.SetActiveCamera(virtualCam);

            Behaviour confiner = virtualCam.GetComponent<CinemachineConfiner>();
            if (confiner == null) confiner = virtualCam.GetComponent("CinemachineConfiner2D") as Behaviour;
            
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
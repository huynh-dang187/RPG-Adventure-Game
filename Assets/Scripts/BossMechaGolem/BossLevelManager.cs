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
    [SerializeField] private float zoomSize = 2f; // Zoom sát vào (số nhỏ)
    [SerializeField] private float zoomSpeed = 1.5f;
    [SerializeField] private float waitTime = 3f;

    private bool isCutscenePlayed = false;

    private void Start()
    {
        if (exitDoor != null) exitDoor.SetActive(false);
        if (camControl == null) camControl = FindObjectOfType<CameraController>();
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
            // --- [QUAN TRỌNG] ---
            // Bắt CameraController phải dùng VCam_Room2
            camControl.SetActiveCamera(virtualCam);
            // --------------------

            // 1. Tắt Confiner (để không kẹt tường)
            Behaviour confiner = virtualCam.GetComponent<CinemachineConfiner>();
            if (confiner == null) confiner = virtualCam.GetComponent("CinemachineConfiner2D") as Behaviour;
            
            if (confiner != null) confiner.enabled = false;

            // 2. Zoom vào
            camControl.SetCameraTarget(zoomTarget);
            camControl.ZoomTo(zoomSize, zoomSpeed);

            yield return new WaitForSeconds(waitTime);

            // 3. Trả về
            camControl.SetPlayerCameraFollow();
            camControl.ResetZoom(zoomSpeed);

            yield return new WaitForSeconds(zoomSpeed);
            
            // 4. Bật lại giới hạn
            if (confiner != null) confiner.enabled = true;
        }
    }
}
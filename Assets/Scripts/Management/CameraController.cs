using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour 
{
    private float defaultSize = 8f; 
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private Coroutine zoomCoroutine;

    // Hàm này để RoomManager/BossManager gọi, ép dùng camera cụ thể
    public void SetActiveCamera(CinemachineVirtualCamera newCam) {
        if (newCam != null) {
            cinemachineVirtualCamera = newCam;
            defaultSize = newCam.m_Lens.OrthographicSize;
            
            // --- [THÊM MỚI QUAN TRỌNG] ---
            // Ngay khi nhận camera mới, bắt nó bám theo Player liền!
            if (PlayerController.Instance != null) {
                cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
            }
            // -----------------------------
        }
    }

    public void SetPlayerCameraFollow()
    {
        // Nếu đang nắm camera nào đó, bắt nó bám theo Player
        if (cinemachineVirtualCamera != null && PlayerController.Instance != null) {
            cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
        }
    }

    // (Giữ nguyên các hàm ZoomTo, ResetZoom, SetCameraTarget bên dưới...)
    public void SetCameraTarget(Transform targetTransform)
    {
        if (cinemachineVirtualCamera != null) {
            cinemachineVirtualCamera.Follow = targetTransform;
        }
    }

    public void ZoomTo(float targetSize, float duration)
    {
        if (zoomCoroutine != null) StopCoroutine(zoomCoroutine);
        zoomCoroutine = StartCoroutine(ZoomProcess(targetSize, duration));
    }

    public void ResetZoom(float duration)
    {
        if (zoomCoroutine != null) StopCoroutine(zoomCoroutine);
        zoomCoroutine = StartCoroutine(ZoomProcess(defaultSize, duration));
    }

    private IEnumerator ZoomProcess(float targetSize, float timeToProcess)
    {
        if (cinemachineVirtualCamera == null) yield break;

        float startSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        float elapsed = 0f;

        while (elapsed < timeToProcess)
        {
            cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, elapsed / timeToProcess);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cinemachineVirtualCamera.m_Lens.OrthographicSize = targetSize;
    }
}
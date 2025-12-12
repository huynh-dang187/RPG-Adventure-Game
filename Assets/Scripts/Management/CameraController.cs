using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour 
{
    private float defaultSize = 8f; 
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private Coroutine zoomCoroutine;

    private void Start()
    {
        // Tự tìm tạm một cái camera để tránh lỗi null lúc đầu
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        if (cinemachineVirtualCamera != null) {
            defaultSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        }
    }

    // [MỚI] Hàm này để BossLevelManager chỉ định chính xác camera nào cần zoom
    public void SetActiveCamera(CinemachineVirtualCamera newCam) {
        if (newCam != null) {
            cinemachineVirtualCamera = newCam;
            // Cập nhật lại size mặc định theo camera mới này
            defaultSize = newCam.m_Lens.OrthographicSize;
        }
    }

    public void SetPlayerCameraFollow()
    {
        if (PlayerController.Instance != null && cinemachineVirtualCamera != null) {
            cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
        }
    }

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
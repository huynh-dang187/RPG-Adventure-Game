using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraController : Singleton<CameraController>
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    
    [Header("Cài đặt Mặc định")]
    [SerializeField] private float defaultSize = 8f; 

    private Coroutine zoomCoroutine;

    // --- SỬA LỖI Ở ĐÂY: XÓA HÀM AWAKE, CHUYỂN HẾT VÀO START ---
    // Để cho Singleton của bạn có thời gian khởi tạo Instance trước
    private void Start()
    {
        // 1. Tìm Camera
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        
        if (cinemachineVirtualCamera != null)
        {
            // Reset size về chuẩn ngay khi bắt đầu
            cinemachineVirtualCamera.m_Lens.OrthographicSize = defaultSize;
        }

        // 2. Bắt đầu follow nhân vật
        SetPlayerCameraFollow();
    }

    public void SetPlayerCameraFollow()
    {
        if(cinemachineVirtualCamera == null) 
            cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        if (PlayerController.Instance != null && cinemachineVirtualCamera != null)
        {
            cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
        }
    }

    public void SetCameraTarget(Transform targetTransform)
    {
        if (cinemachineVirtualCamera != null)
        {
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
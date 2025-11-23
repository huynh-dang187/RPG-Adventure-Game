using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossAreaExit : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;

    [Header("Animation Settings")]
    [Tooltip("Thời gian bị hút vào cổng (giây)")]
    [SerializeField] private float suckDuration = 1f;
    [Tooltip("Thời gian chờ màn hình đen trước khi load scene")]
    [SerializeField] private float waitToLoadTime = 0.5f;
    [Tooltip("Có xoay khi bị hút không?")]
    [SerializeField] private bool spinWhileSucking = true;

    private bool isActivated = false; // Để tránh kích hoạt 2 lần

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu là Player VÀ chưa kích hoạt cổng lần nào
        if (other.CompareTag("Player") && !isActivated)
        {
            PlayerController playerScript = other.GetComponent<PlayerController>();
            if (playerScript != null)
            {
                isActivated = true; // Đánh dấu đã dùng cổng
                SoundManager.Instance.PlaySound3D("Portal", transform.position);

                // Bắt đầu Coroutine hút Player
                StartCoroutine(SuckPlayerInRoutine(other.transform, playerScript));
            }
        }
    }

    private IEnumerator SuckPlayerInRoutine(Transform playerTransform, PlayerController playerScript)
    {
        // 1. TẮT ĐIỀU KHIỂN PLAYER NGAY LẬP TỨC
        playerScript.enabled = false;
        // (Nếu bạn có component Rigidbody2D, nên set nó thành Kinematic để tránh vật lý lạ)
        Rigidbody2D rb = playerTransform.GetComponent<Rigidbody2D>();
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;

        // Lưu lại trạng thái ban đầu
        Vector3 startPos = playerTransform.position;
        Vector3 startScale = playerTransform.localScale;
        float timer = 0f;

        // 2. VÒNG LẶP HIỆU ỨNG "HÚT VÀO"
        while (timer < suckDuration)
        {
            timer += Time.deltaTime;
            float t = timer / suckDuration; // Giá trị từ 0 đến 1

            // a. Di chuyển dần về tâm cổng (dùng Lerp cho mượt)
            playerTransform.position = Vector3.Lerp(startPos, transform.position, t);

            // b. Thu nhỏ dần về 0
            playerTransform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

            // c. Xoay vòng tròn (nếu bật) - Xoay cực nhanh
            if (spinWhileSucking)
            {
                playerTransform.Rotate(0, 0, 720 * Time.deltaTime);
            }

            yield return null; // Chờ đến frame tiếp theo
        }

        // Đảm bảo Player biến mất hẳn khi kết thúc
        playerTransform.localScale = Vector3.zero;
        playerTransform.position = transform.position;

        // 3. BẮT ĐẦU FADE ĐEN MÀN HÌNH
        SceneManagement.Instance.SetTransitionName(sceneTransitionName);
        UIFade.Instance.FadeToBlack();

        // 4. CHỜ FADE XONG RỒI LOAD SCENE
        yield return new WaitForSeconds(waitToLoadTime);
        SceneManager.LoadScene(sceneToLoad);
    }
}
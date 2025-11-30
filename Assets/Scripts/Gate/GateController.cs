using UnityEngine;
using UnityEngine.SceneManagement; // Để chuyển Scene

public class GateController : MonoBehaviour
{
    [Header("Cài đặt")]
    [SerializeField] private GameObject blueLight; // Kéo cái hình đèn xanh vào đây
    [SerializeField] private string nextSceneName = "Scene4"; // Tên scene muốn qua
    [SerializeField] private bool isOpen = false;

    private void Start()
    {
        // Mặc định tắt đèn, đóng cổng
        if (blueLight != null) blueLight.SetActive(false);
        isOpen = false;
    }

    // Hàm này sẽ được EnemyManager gọi khi hết quái
    public void OpenGate()
    {
        isOpen = true;
        if (blueLight != null) blueLight.SetActive(true); // Bật đèn lên
        
        // Có thể thêm âm thanh mở cổng ở đây

        //SoundManager.Instance.PlaySound3D("GateOpen", transform.position);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu cổng đã mở VÀ người chạm vào là Player
        if (isOpen && other.CompareTag("Player"))
        {
            Debug.Log("Chuyển Scene!");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
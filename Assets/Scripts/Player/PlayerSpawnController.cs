using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnController : MonoBehaviour
{
    [Header("Hiệu ứng Xuất hiện")]
    public GameObject spawnVFXPrefab; 
    public float delayBeforeShowingPlayer = 1.0f;

    [Header("Căn chỉnh vị trí VFX")]
    [Tooltip("Chỉnh số Y lên xuống để cột sáng khớp với người")]
    public Vector3 vfxOffset = new Vector3(0, 0.5f, 0); // Mặc định thử nhích lên 0.5

    private SpriteRenderer mySprite;
    private PlayerController playerController;
    private Transform weaponHolder; 

    private void Awake()
    {
        mySprite = GetComponentInChildren<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();

        // Tìm object chứa vũ khí (nhớ kiểm tra tên cho đúng)
        weaponHolder = transform.Find("Active Weapon");
        if (weaponHolder == null) weaponHolder = transform.Find("ActiveWeapon");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(SpawnSequence());
    }

    IEnumerator SpawnSequence()
    {
        yield return null; 

        // TÀNG HÌNH & KHÓA
        if (mySprite != null) mySprite.enabled = false; 
        if (playerController != null) playerController.isLocked = true;
        if (weaponHolder != null) weaponHolder.gameObject.SetActive(false);

        // --- TẠO HIỆU ỨNG (CÓ CỘNG THÊM OFFSET) ---
        if (spawnVFXPrefab != null)
        {
            // Vị trí sinh ra = Chân Player + Độ lệch (Offset)
            Instantiate(spawnVFXPrefab, transform.position + vfxOffset, Quaternion.identity);
        }
        // ------------------------------------------

        yield return new WaitForSeconds(delayBeforeShowingPlayer);

        // HIỆN HÌNH & MỞ KHÓA
        if (mySprite != null) mySprite.enabled = true;
        if (playerController != null) playerController.isLocked = false;
        if (weaponHolder != null) weaponHolder.gameObject.SetActive(true);
    }
}
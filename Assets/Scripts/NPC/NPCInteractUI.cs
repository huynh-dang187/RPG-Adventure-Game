using UnityEngine;

public class NPCInteractUI : MonoBehaviour
{
    public GameObject interactPrefab;   // Prefab chữ E (UI)
    public Transform interactPoint;     // điểm hiển thị trên đầu NPC
    public float showDistance = 2f;

    private GameObject interactUI;
    private Transform player;
    private Canvas mainCanvas;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        mainCanvas = FindObjectOfType<Canvas>();

        // Tạo UI trong Canvas
        interactUI = Instantiate(interactPrefab, mainCanvas.transform);
        interactUI.SetActive(false);
    }

    void Update()
    {
        float dist = Vector2.Distance(player.position, transform.position);

        if (dist < showDistance)
        {
            interactUI.SetActive(true);

            // convert vị trí NPC sang vị trí màn hình
            Vector3 pos = Camera.main.WorldToScreenPoint(interactPoint.position);
            interactUI.transform.position = pos;
        }
        else
        {
            interactUI.SetActive(false);
        }
    }
}

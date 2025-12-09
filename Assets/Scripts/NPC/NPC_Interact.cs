using UnityEngine;
using UnityEngine.Events; // <--- 1. BẮT BUỘC THÊM THƯ VIỆN NÀY

public class NPC_Interact : MonoBehaviour
{
    public Dialogue dialogueData; 
    public GameObject pressE_Icon; 

    [Header("Sự kiện sau khi nói xong")]
    public UnityEvent onDialogueFinish; // <--- 2. THÊM BIẾN NÀY ĐỂ KÉO THẢ TRONG UNITY

    private bool isPlayerInRange;
    private bool hasSpoken = false; 

    void Update()
    {
        if (hasSpoken) 
        {
            if(pressE_Icon != null) pressE_Icon.SetActive(false);
            return;
        }

        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!DialogueManager.Instance.dialoguePanel.activeInHierarchy)
            {
                // Gọi Manager và truyền hàm OnDialogueFinished vào làm callback
                DialogueManager.Instance.StartDialogue(dialogueData, OnDialogueFinished);
            }
        }
    }

    // Hàm này được DialogueManager gọi lại khi hết thoại
    void OnDialogueFinished()
    {
        hasSpoken = true; 
        if (pressE_Icon != null)
        {
            pressE_Icon.SetActive(false); 
        }

        // <--- 3. KÍCH HOẠT SỰ KIỆN INSPECTOR TẠI ĐÂY
        // Bất cứ thứ gì bạn kéo vào Inspector sẽ chạy ngay lúc này
        onDialogueFinish?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (!hasSpoken && pressE_Icon) pressE_Icon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (pressE_Icon) pressE_Icon.SetActive(false);
        }
    }
}
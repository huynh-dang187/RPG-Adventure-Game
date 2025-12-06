using UnityEngine;

public class NPC_Interact : MonoBehaviour
{
    public Dialogue dialogueData; 
    public GameObject pressE_Icon; 

    private bool isPlayerInRange;
    private bool hasSpoken = false; // Biến ghi nhớ đã nói chuyện xong chưa

    void Update()
    {
        // Nếu đã nói xong rồi thì không hiện E và không cho nói nữa (hoặc tùy bạn)
        if (hasSpoken) 
        {
            if(pressE_Icon != null) pressE_Icon.SetActive(false);
            return;
        }

        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!DialogueManager.Instance.dialoguePanel.activeInHierarchy)
            {
                // Gọi Manager mở thoại và truyền thêm hàm xử lý kết thúc
                DialogueManager.Instance.StartDialogue(dialogueData, OnDialogueFinished);
            }
        }
    }

    // Hàm này sẽ được DialogueManager gọi tự động khi bấm E hết câu cuối
    void OnDialogueFinished()
    {
        hasSpoken = true; // Đánh dấu là đã nói xong
        if (pressE_Icon != null)
        {
            pressE_Icon.SetActive(false); // Ẩn icon E đi
            // Destroy(pressE_Icon); // Nếu muốn xóa vĩnh viễn khỏi game thì dùng dòng này
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            // Chỉ hiện E nếu CHƯA nói chuyện
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System; // Để dùng Action callback

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI Components")]
    public GameObject dialoguePanel;
    public Image portraitImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject arrowIcon;

    [Header("Settings")]
    public float typingSpeed = 0.02f;

    private Queue<string> sentences;
    private bool isTyping = false;
    private string currentSentence = "";
    private bool canSkip = false;

    private PlayerController player;
    
    // --- MỚI: Sự kiện để báo lại cho NPC khi nói xong ---
    private Action onDialogueEndedCallback; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        sentences = new Queue<string>();
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        dialoguePanel.SetActive(false);
    }

    // Thêm tham số callback vào hàm StartDialogue
    public void StartDialogue(Dialogue dialogue, Action onEnded = null)
    {
        if (player != null) player.isLocked = true;
        
        // Lưu lại hành động cần làm khi kết thúc (ví dụ: xóa icon E)
        onDialogueEndedCallback = onEnded;

        dialoguePanel.SetActive(true);
        nameText.text = dialogue.npcName;
        portraitImage.sprite = dialogue.portrait;

        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        canSkip = false;
        StartCoroutine(EnableSkip());

        DisplayNextSentence();
    }

    IEnumerator EnableSkip()
    {
        yield return null;
        canSkip = true;
    }

    public void DisplayNextSentence()
    {
        // 1. Nếu đang chạy chữ -> Hiện full (Skip)
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentSentence;
            isTyping = false;
            if(arrowIcon) arrowIcon.SetActive(true);
            return;
        }

        // 2. Kiểm tra xem còn câu nào không
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        // 3. Hiện câu tiếp theo
        currentSentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(currentSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        if(arrowIcon) arrowIcon.SetActive(false);

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        if(arrowIcon) arrowIcon.SetActive(true);
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        
        if (player != null) player.isLocked = false;

        // --- MỚI: Gọi sự kiện kết thúc (báo cho NPC biết) ---
        if (onDialogueEndedCallback != null)
        {
            onDialogueEndedCallback.Invoke();
            onDialogueEndedCallback = null; // Reset lại
        }
    }

    void Update()
    {
        if (dialoguePanel.activeInHierarchy && canSkip)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                DisplayNextSentence();
            }
            // Bấm ESC cũng tắt luôn
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndDialogue();
            }
        }
    }
}
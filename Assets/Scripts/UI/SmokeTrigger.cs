using UnityEngine;
using System.Collections;

public class SmokeTrigger : MonoBehaviour
{
    [Header("Cài đặt Object")]
    public GameObject punctuationIcon; 
    
    [Header("Cài đặt hiển thị")]
    public Vector3 offset = new Vector3(0, 1.5f, 0); 
    public Vector3 iconScale = new Vector3(0.15f, 0.15f, 1f);

    [Header("Cài đặt thời gian")]
    [Tooltip("Thời gian hiện dấu chấm than (giây)")]
    public float displayDuration = 2.0f; // <--- BIẾN MỚI Ở ĐÂY (Mặc định 2 giây)

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Player") || (other.transform.parent != null && other.transform.parent.CompareTag("Player"))) 
            && !hasTriggered)
        {
            hasTriggered = true;

            Transform playerTransform = other.transform;
            if (!playerTransform.CompareTag("Player") && playerTransform.parent != null)
            {
                playerTransform = playerTransform.parent;
            }

            if (punctuationIcon != null)
            {
                punctuationIcon.transform.SetParent(playerTransform);
                punctuationIcon.transform.localPosition = offset;
                punctuationIcon.transform.localScale = iconScale; 

                punctuationIcon.SetActive(true);

                // Gọi coroutine tắt theo thời gian đã cài
                StartCoroutine(HideAlertRoutine());
            }
        }
    }

    IEnumerator HideAlertRoutine()
    {
        // Sử dụng biến displayDuration thay vì số cứng 2f
        yield return new WaitForSeconds(displayDuration); 
        
        if (punctuationIcon != null)
        {
            punctuationIcon.SetActive(false);
        }
    }
}
using System.Collections;
using UnityEngine;

public class SkeletonFlash : MonoBehaviour
{
    [SerializeField] private float flashTime = 0.1f;
    [SerializeField] private Color flashColor = Color.red;

    private SpriteRenderer[] allRenderers; // Lấy TẤT CẢ các bộ phận (tay, chân, đầu)
    private Color[] originalColors;

    private void Awake() {
        // Tìm tất cả SpriteRenderer trong cả cha và con
        allRenderers = GetComponentsInChildren<SpriteRenderer>();
        
        // Lưu lại màu gốc của từng bộ phận
        originalColors = new Color[allRenderers.Length];
        for (int i = 0; i < allRenderers.Length; i++)
        {
            originalColors[i] = allRenderers[i].color;
        }
    }

    public float GetRestoreMatTime() {
        return flashTime;
    }

    public IEnumerator FlashRoutine() {
        // 1. Nhuộm đỏ TẤT CẢ bộ phận
        foreach (var sr in allRenderers)
        {
            sr.color = flashColor;
        }

        // 2. Đợi
        yield return new WaitForSeconds(flashTime);

        // 3. Trả lại màu gốc cho TẤT CẢ
        for (int i = 0; i < allRenderers.Length; i++)
        {
            allRenderers[i].color = originalColors[i];
        }
    }
}
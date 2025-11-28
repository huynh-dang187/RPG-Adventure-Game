using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material whiteFlashMat;
    [SerializeField] private float restoreMatTime = .2f;

    private Material defaultMat;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        // QUAN TRỌNG: Tìm SpriteRenderer ở object CON (blueSmile)
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); 

        if (spriteRenderer != null)
        {
            defaultMat = spriteRenderer.material;
        }
    }

    public float GetRestoreMatTime() {
        return restoreMatTime;
    }

    public IEnumerator FlashRoutine() {
        if (spriteRenderer != null && whiteFlashMat != null) 
        {
            // Đổi sang màu trắng
            spriteRenderer.material = whiteFlashMat;
            // Đợi xíu
            yield return new WaitForSeconds(restoreMatTime);
            // Trả về màu cũ
            spriteRenderer.material = defaultMat;
        }
    }
}
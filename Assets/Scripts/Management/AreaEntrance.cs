using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [Header("Cài đặt Cổng")]
    [SerializeField] private string transitionName;

    [Header("Cài đặt Nhân vật")]
    [Tooltip("Kích thước nhân vật khi vào Scene này (Mặc định là 1,1,1)")]
    public Vector3 playerScale = new Vector3(1f, 1f, 1f);

    private void Start() {
        // --- LUÔN RESET TRẠNG THÁI PLAYER ---
        if (PlayerController.Instance != null) {
            
            // 1. Bật lại điều khiển
            PlayerController.Instance.enabled = true; 

            // 2. Reset Kích thước
            PlayerController.Instance.transform.localScale = playerScale; 

            PlayerController.Instance.transform.rotation = Quaternion.identity;

            // 3. Reset Vật lý
            Rigidbody2D rb = PlayerController.Instance.GetComponent<Rigidbody2D>();
            if (rb != null) {
                rb.bodyType = RigidbodyType2D.Dynamic; 
                rb.linearVelocity = Vector2.zero; 
            }

            // ==========================================================
            // [THÊM MỚI] 4. QUAN TRỌNG: RESET LAYER VỀ "PLAYER"
            // ==========================================================
            // Đảm bảo tên layer trong Unity của bạn là "Player" (viết hoa chữ P)
            PlayerController.Instance.gameObject.layer = LayerMask.NameToLayer("Default");
            Debug.Log("AreaEntrance: Đã reset Layer nhân vật về Player!");
            // ==========================================================

            // 5. Reset Animation
            Animator anim = PlayerController.Instance.GetComponent<Animator>();
            if (anim != null) {
                anim.SetFloat("moveX", 0);
                anim.SetFloat("moveY", 0);
                anim.SetBool("isMoving", false); 
                anim.Play("Idle"); 
            }
        }

        // Logic Set Vị trí (chỉ chạy khi đúng tên cổng)
        if (transitionName == SceneManagement.Instance.SceneTransitionName) {
            PlayerController.Instance.transform.position = this.transform.position;
            CameraController.Instance.SetPlayerCameraFollow();
            UIFade.Instance.FadeToClear();
            SoundManager.Instance.PlaySound3D("Portal", transform.position);
        }
    }
}
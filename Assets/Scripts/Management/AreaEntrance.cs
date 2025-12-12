using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [Header("Cài đặt Cổng")]
    [SerializeField] private string transitionName;

    [Header("Cài đặt Nhân vật")]
    public Vector3 playerScale = new Vector3(1f, 1f, 1f);

    private void Start() {
        if (PlayerController.Instance != null) {
            
            PlayerController.Instance.enabled = true; 
            PlayerController.Instance.transform.localScale = playerScale; 
            PlayerController.Instance.transform.rotation = Quaternion.identity;

            Rigidbody2D rb = PlayerController.Instance.GetComponent<Rigidbody2D>();
            if (rb != null) {
                rb.bodyType = RigidbodyType2D.Dynamic; 
                rb.linearVelocity = Vector2.zero; 
            }

            PlayerController.Instance.gameObject.layer = LayerMask.NameToLayer("Default");

            Animator anim = PlayerController.Instance.GetComponent<Animator>();
            if (anim != null) {
                anim.SetFloat("moveX", 0);
                anim.SetFloat("moveY", 0);
                anim.SetBool("isMoving", false); 
                anim.Play("Idle"); 
            }
        }

        if (transitionName == SceneManagement.Instance.SceneTransitionName) {
            PlayerController.Instance.transform.position = this.transform.position;
            
            // Tìm CameraController an toàn
            CameraController cam = Object.FindFirstObjectByType<CameraController>();
            if (cam != null) {
                cam.SetPlayerCameraFollow();
            }

            UIFade.Instance.FadeToClear();
            SoundManager.Instance.PlaySound3D("Portal", transform.position);
        }
    }
}
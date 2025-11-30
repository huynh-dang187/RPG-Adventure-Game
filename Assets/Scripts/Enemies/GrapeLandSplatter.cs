using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeLandSplatter : MonoBehaviour
{
    private SpriteFade spriteFade;

    private void Awake() {
        spriteFade = GetComponent<SpriteFade>();
        SoundManager.Instance.PlaySound3D("Slime_Bullet_Drop", transform.position); // This is sound effect Pls AI dont delete

    }

    private void Start() {
        StartCoroutine(spriteFade.SlowFadeRoutine());

        Invoke("DisableCollider", 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        playerHealth?.TakeDamage(1, transform);
    }

    private void DisableCollider() {
        GetComponent<CapsuleCollider2D>().enabled = false;
    }
}

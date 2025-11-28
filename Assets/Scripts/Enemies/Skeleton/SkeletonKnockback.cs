using System.Collections;
using UnityEngine;

public class SkeletonKnockback : MonoBehaviour
{
    // Biến này để báo cho EnemyAI biết là "Tao đang bị đẩy, đừng có di chuyển"
    public bool GettingKnockedBack { get; private set; }

    [SerializeField] private float knockBackTime = .2f; // Thời gian bị đẩy lùi

    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedBack(Transform damageSource, float knockBackThrust) {
        GettingKnockedBack = true;
        
        // 1. Tính hướng đẩy: Từ nguồn sát thương -> Về phía quái
        // (normalized để lấy hướng, nhân với lực đẩy, nhân với khối lượng để đẩy được cả quái nặng)
        Vector2 difference = (transform.position - damageSource.position).normalized * knockBackThrust * rb.mass;
        
        // 2. Reset vận tốc cũ để lực đẩy có tác dụng ngay lập tức
        rb.linearVelocity = Vector2.zero; // Unity 6 (bản cũ là .velocity)
        
        // 3. Thêm lực đẩy
        rb.AddForce(difference, ForceMode2D.Impulse); 
        
        // 4. Bắt đầu đếm ngược thời gian dừng đẩy
        StartCoroutine(KnockRoutine());
    }

    private IEnumerator KnockRoutine() {
        yield return new WaitForSeconds(knockBackTime);
        
        // Hết thời gian đẩy -> Dừng lại -> Cho phép AI di chuyển tiếp
        rb.linearVelocity = Vector2.zero;
        GettingKnockedBack = false;
    }
}
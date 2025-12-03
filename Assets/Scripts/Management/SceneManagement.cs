using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Đảm bảo class Singleton của bạn hoạt động đúng, hoặc dùng code dưới đây để ghi đè cho chắc
public class SceneManagement : Singleton<SceneManagement>
{
    public string SceneTransitionName { get; private set; }

    // --- THÊM ĐOẠN NÀY ĐỂ BẤT TỬ KHI QUA MÀN ---
    protected override void Awake() 
    {
        base.Awake(); // Gọi hàm khởi tạo của Singleton cha
        DontDestroyOnLoad(gameObject); // Giữ object này lại không cho Unity hủy khi qua màn
    }
    // -------------------------------------------

    public void SetTransitionName(string sceneTransitionName) {
        this.SceneTransitionName = sceneTransitionName;
    }
}
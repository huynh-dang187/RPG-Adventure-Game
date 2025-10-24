using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T> 
{
    private static T instance;
    public static T Instance { get { return instance; } }

    protected virtual void Awake()
{
    if (instance != null && instance != this)
    {
        // Chỉ hủy sau frame này để tránh OnDisable() bị gọi giữa lúc khởi tạo
        Destroy(gameObject, 0.01f);
        return;
    }

    instance = (T)this;
    DontDestroyOnLoad(gameObject);
}

}

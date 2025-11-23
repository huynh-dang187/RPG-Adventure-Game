using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;

    
    [SerializeField] private float waitToLoadTime = 0.5f; 

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<PlayerController>()) {
            
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);
            
            // Bắt đầu làm tối màn hình
            UIFade.Instance.FadeToBlack();
            
            StartCoroutine(LoadSceneRoutine());
            SoundManager.Instance.PlaySound3D("Portal", transform.position); 
        }
    }

    private IEnumerator LoadSceneRoutine(){
        // 3. Thay vòng lặp while dài dòng bằng 1 dòng này
        // Nó sẽ đợi đúng khoảng thời gian bạn cài đặt rồi chạy tiếp
        yield return new WaitForSeconds(waitToLoadTime);

        SceneManager.LoadScene(sceneToLoad);
    }
}
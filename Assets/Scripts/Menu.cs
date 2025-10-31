using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Simple: load the next scene by build index
    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    // Quit the application. While testing in the Editor, stop play mode.
    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif  
    }
}

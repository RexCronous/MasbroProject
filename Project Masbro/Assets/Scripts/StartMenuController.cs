using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void OnStartClick()
    {
        print("start");
        Time.timeScale = 1f;
        SceneManager.LoadScene("SelectLevel");
    }

    public void OnExitClick()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        print("exit");
    }
}

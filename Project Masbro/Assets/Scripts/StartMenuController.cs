using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    private int levelUnlocked;

    public void OnStartClick()
    {
        print("start");
        Time.timeScale = 1f;

        levelUnlocked = PlayerPrefs.GetInt("LevelUnlocked", 0);

        if (levelUnlocked == 0)
        {
            SceneManager.LoadScene("TutorialLevel");
        }
        else
        {
            SceneManager.LoadScene("SelectLevel");
        }
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

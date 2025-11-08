using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject SettingsPanel;

    private int tutorialDone;

    private void Start()
    {
        if (SettingsPanel != null)
        {
            SettingsPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Settings Panel is not assigned in StartMenuController!");
        }
    }

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

    public void OnSettingClick()
    {
        print("setting");
        // Time.timeScale = 1f;
        SettingsPanel.SetActive(true);
    }

    public void OnBackClick()
    {
        SettingsPanel.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject SettingsPanel;

    private AudioManager audioManager;

    [System.Obsolete]
    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        FindObjectOfType<VolumeSettings>()?.LoadVolume();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager tidak ditemukan di scene!");
        }

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
        if (audioManager != null && audioManager.interactItemGameOverMenu != null)
        {
            audioManager.PlaySfx(audioManager.interactItemGameOverMenu);
        }
        Time.timeScale = 1f;
        // Panggil Coroutine untuk Fade Out dan Load Scene
        StartCoroutine(FadeOutAndLoadScene("SelectLevel"));
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        if (audioManager != null)
        {
            // Mulai Fade Out dan TUNGGU sampai Coroutine FadeOutMusic selesai
            yield return StartCoroutine(audioManager.FadeOutMusic());
        }

        // Setelah musik benar-benar sunyi (atau jika AudioManager tidak ada),
        // barulah scene dimuat.
        SceneManager.LoadScene(sceneName);
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
        if (audioManager != null && audioManager.interactItemGameOverMenu != null)
        {
            audioManager.PlaySfx(audioManager.interactItemGameOverMenu);
        }
        // Time.timeScale = 1f;
        SettingsPanel.SetActive(true);
    }

    public void OnBackClick()
    {
        Debug.Log("back");
        if (audioManager != null && audioManager.interactItemGameOverMenu != null)
        {
            audioManager.PlaySfx(audioManager.interactItemGameOverMenu);
        }
        SettingsPanel.SetActive(false);
    }
}

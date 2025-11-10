using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject SettingsPanel;

    private bool isTranstioning = false;

    private AudioManager audioManager;

    [System.Obsolete]
    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        FindObjectOfType<VolumeSettings>()?.LoadVolume();

        if (SettingsPanel != null)
        {
            SettingsPanel.SetActive(false);
        }
    }

    public void OnStartClick()
    {
        if (isTranstioning)
        {
            return;
        }
        isTranstioning = true;
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
        if (isTranstioning)
        {
            return;
        }
        isTranstioning = true;
        print("exit");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

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
        if (audioManager != null && audioManager.interactItemGameOverMenu != null)
        {
            audioManager.PlaySfx(audioManager.interactItemGameOverMenu);
        }
        SettingsPanel.SetActive(false);
    }
}
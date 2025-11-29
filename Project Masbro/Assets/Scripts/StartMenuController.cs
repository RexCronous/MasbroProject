using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private VolumeSettings volumeSettings; // Assign dari inspector

    private bool isTranstioning = false;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
    }

    // [System.Obsolete]
    private void Start()
    {
        if (audioManager == null)
            audioManager = FindFirstObjectByType<AudioManager>();
        
        volumeSettings?.LoadVolume();

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
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }

    public void OnSettingClick()
    {
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
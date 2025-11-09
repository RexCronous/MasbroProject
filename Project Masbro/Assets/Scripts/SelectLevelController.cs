using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SelectLevelController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private GameObject tutorialPanel;
    [Header("Buttons")]
<<<<<<< HEAD
    [SerializeField] private GameObject[] levelButtons;
=======
    [SerializeField] private Button[] levelButtons;
    private AudioManager audioManager;

    private int levelUnlocked;
>>>>>>> 83cda8ff9085491c2a5c609d6ce05efd46bc11ac

    [System.Obsolete]

    private void Start()
    {

        // Ensure default unlock(s) are initialized in one place (menu)
        if (!PlayerPrefs.HasKey("Level1_Unlocked"))
        {
            PlayerPrefs.SetInt("Level1_Unlocked", 1);
            PlayerPrefs.Save();
        }

        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager tidak ditemukan di scene!");
        }

        if (levelButtons != null && levelButtons.Length > 0)
        {
            // Check setiap level button
            for (int i = 0; i < levelButtons.Length; i++)
            {
                int levelNum = i + 1;
                if (levelButtons[i] != null)
                {
                    Button btn = levelButtons[i].GetComponent<Button>();
                    if (btn != null)
                    {
                        bool isUnlocked = (levelNum == 1) || (PlayerPrefs.GetInt($"Level{levelNum}_Unlocked", 0) == 1);
                        btn.interactable = isUnlocked;

                        Debug.Log($"Level {levelNum} - Unlock Status: {isUnlocked}");
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("Level buttons array is empty or not assigned in SelectLevelController!");
        }

        confirmPanel.SetActive(false);
        tutorialPanel.SetActive(false);
    }

    public void OnRestartClick()
    {
        if (confirmPanel != null)
        {
            confirmPanel.SetActive(true);
            mainPanel.SetActive(false);
        }
    }

    public void OnConfirmYes(GameObject panel)
    {
        if (panel == confirmPanel)
        {
            // Reset semua progress
            for (int i = 1; i <= levelButtons.Length; i++)
            {
                // Hapus status unlock level (kecuali level 1)
                if (i > 1)
                {
                    PlayerPrefs.DeleteKey($"Level{i}_Unlocked");
                }

                // Hapus progress bintang
                PlayerPrefs.DeleteKey($"Level{i}_Progress");

                // Hapus waktu tercepat
                PlayerPrefs.DeleteKey($"FastestTime_Level{i}");
            }

            // Set ulang level 1 ke terbuka
            PlayerPrefs.SetInt("Level1_Unlocked", 1);

            // Set Tutorial
            PlayerPrefs.SetInt("Tutorial_Done", 0);

            // Pastikan perubahan tersimpan
            PlayerPrefs.Save();

            Debug.Log("Semua progress level telah direset");

            // Reload scene
            SceneManager.LoadScene("Scenes/SelectLevel");
        }
        else if (panel == tutorialPanel)
        {
            SceneManager.LoadScene("TutorialLevel");
        }
    }

    public void OnConfirmNo(GameObject panel)
    {
        if (panel == tutorialPanel)
        {
            tutorialPanel.SetActive(false);
        }
        confirmPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        if (audioManager != null && audioManager.interactItemGameOverMenu != null)
        {
            audioManager.PlaySfx(audioManager.interactItemGameOverMenu);
        }
        StartCoroutine(FadeOutAndLoadScene("MainMenu"));
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
}

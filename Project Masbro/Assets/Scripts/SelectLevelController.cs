using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevelController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject confirmPanel;
    [Header("Buttons")]
    [SerializeField] private Button[] levelButtons;

    private int levelUnlocked;

    private void Start()
    {
        if (levelButtons != null && levelButtons.Length > 0)
        {
            // Check setiap level button
            for (int i = 0; i < levelButtons.Length; i++)
            {
                int levelNum = i + 1;
                if (levelButtons[i] != null)
                {
                    // Cek status unlock untuk setiap level
                    bool isUnlocked = (levelNum == 1) || (PlayerPrefs.GetInt($"Level{levelNum}_Unlocked", 0) == 1);
                    levelButtons[i].interactable = isUnlocked;

                    Debug.Log($"Level {levelNum} - Unlock Status: {isUnlocked}");
                }
            }
        }
        else
        {
            Debug.LogWarning("Level buttons array is empty or not assigned in SelectLevelController!");
        }

        confirmPanel.SetActive(false);
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    public void OnRestartClick()
    {
        if (confirmPanel != null)
        {
            confirmPanel.SetActive(true);
            MainPanel.SetActive(false);
        }
    }

    public void OnConfirmYes()
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

        // Pastikan perubahan tersimpan
        PlayerPrefs.Save();

        Debug.Log("Semua progress level telah direset");

        // Reload scene
        SceneManager.LoadScene("Scenes/SelectLevel");
    }

    public void OnConfirmNo()
    {
        if (confirmPanel != null)
        {
            confirmPanel.SetActive(false);
            MainPanel.SetActive(true);
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

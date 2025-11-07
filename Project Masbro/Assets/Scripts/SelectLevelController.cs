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
        levelUnlocked = PlayerPrefs.GetInt("LevelUnlocked", 1);
        print("Level Unlocked: " + levelUnlocked);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 <= levelUnlocked)
            {
                levelButtons[i].interactable = true;
            }
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
        PlayerPrefs.DeleteKey("LevelUnlocked");
        for (int i = 1; i < levelUnlocked; i++)
        {
            PlayerPrefs.DeleteKey("FastestTime_Level" + i);
        }

        SceneManager.LoadScene("MainMenu");
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

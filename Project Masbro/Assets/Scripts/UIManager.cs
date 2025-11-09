using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Tambahkan event publik statis
    public static event Action OnFinishEvent;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;
    [Header("Finish")]
    [SerializeField] private GameObject finishScreen;
    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;
    [Header("Progress")]
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private GameObject SettingsPanel;

    private AudioManager audioManager;

    [System.Obsolete]
    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        // FindObjectOfType<VolumeSettings>()?.LoadVolume();
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
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        finishScreen.SetActive(false);
    }

    #region  Game Over
    public void GameOver()
    {
        if (audioManager != null && audioManager.gameOver != null)
        {
            audioManager.PlaySfx(audioManager.gameOver);
        }
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
        else
        {
            Debug.LogWarning("GameOverSound belum diisi di Inspector!");
        }

    }

    public void Finish()
    {
        if (finishScreen != null && audioManager != null && audioManager.finished != null)
        {
            Time.timeScale = 0;
            audioManager.PlaySfx(audioManager.finished);
            finishScreen.SetActive(true);
            finishScreen.GetComponent<LevelTimer>()?.FinishLevel();

            if (audioManager != null && audioManager.finish != null)
            {
                audioManager.PlaySfx(audioManager.finish);
            }
        }

        OnFinishEvent?.Invoke();
    }

    public void NextLevel()
    {
        int currentSceneIndex = GameManager.Instance.currentSceneIndex;

        currentSceneIndex++;
        if (currentSceneIndex >= SceneManager.sceneCountInBuildSettings - 1)
        {
            currentSceneIndex = 0; // Kembali ke menu utama atau scene pertama
        }

        SceneManager.LoadScene(currentSceneIndex);
    }

    // game over funtion
    public void Restart()
    {
        SceneManager.LoadScene(GameManager.Instance.currentSceneIndex);
        //spawnSystem.SpawnAtStart();// this method is not work fr fr
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    #endregion

    public void UpdateProgress(int checkpointsReached, int totalCheckpoints)
    {
        progressText.text = $"{checkpointsReached}/{totalCheckpoints}";
    }

    public void UpdateTimer(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseScreen.activeInHierarchy)
            {
                PauseGame(false);
                if (audioManager != null && audioManager.openPause != null)
                {
                    audioManager.PlaySfx(audioManager.openPause);
                }
            }
            else
            {
                PauseGame(true);
                if (audioManager != null && audioManager.closedPause != null)
                {
                    audioManager.PlaySfx(audioManager.closedPause);
                }
            }
        }
    }

    #region Pause
    public void PauseGame(bool status)
    {
        pauseScreen.SetActive(status);

        if (status)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void OnSettingClick()
    {
        print("setting");
        // if (audioManager != null && audioManager.interactItemGameOverMenu != null)
        // {
        //     audioManager.PlaySfx(audioManager.interactItemGameOverMenu);
        // }
        // Time.timeScale = 1f;
        pauseScreen.SetActive(false);
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
        pauseScreen.SetActive(true);
    }
    #endregion
}
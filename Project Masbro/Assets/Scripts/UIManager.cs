using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;
    [Header("Finish")]
    [SerializeField] private GameObject finishScreen;
    AudioManager audioManager;

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
        if (finishScreen != null)
        {
            finishScreen.SetActive(true);
            finishScreen.GetComponent<LevelTimer>()?.FinishLevel();
        }
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

    public void MusicVolume()
    {

    }
    public void SoundVolume()
    {

    }
    #endregion
}
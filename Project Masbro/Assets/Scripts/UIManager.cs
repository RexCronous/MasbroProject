using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;
    private AudioSource audioSource;
    private SpawnSystem spawnSystem;

    private void Awake()
    {
        // Ambil AudioSource dari GameObject ini
        audioSource = GetComponent<AudioSource>();

        // Kalau belum ada, tambahkan otomatis
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        gameOverScreen.SetActive(false);
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);

        // Pastikan ada AudioClip sebelum dimainkan
        if (gameOverSound != null)
            audioSource.PlayOneShot(gameOverSound);
        else
            Debug.LogWarning("GameOverSound belum diisi di Inspector!");
    }

    // game over funtion
    public void Restart()
    {
        spawnSystem.SpawnAtStart();// this method is not work fr fr
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

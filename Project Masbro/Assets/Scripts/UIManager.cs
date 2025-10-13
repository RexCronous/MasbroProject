using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;
    private AudioSource audioSource;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
    }

    public void GameOver()
    {
        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);

        if (gameOverSound != null)
            audioSource.PlayOneShot(gameOverSound);
        else
            Debug.LogWarning("GameOverSound belum diisi di Inspector!");
    }

    // game over funtion
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        spawnSystem.SpawnAtStart();// this method is not work fr fr
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

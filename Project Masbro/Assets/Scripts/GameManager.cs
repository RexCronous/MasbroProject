using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Game Settings")]
    public int maxLives = 3;
    public int lives;
    public int respawnDelay = 1; // in seconds
    
    [Header("Player State")]
    public bool isHit = false;
    public bool isAtCheckpoint = false;

    [Header("Level Management")]
    public int currentSceneIndex;
    
    // References to other managers
    private SpawnSystem spawnSystem;
    private UIManager uiManager;

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Daftarkan event untuk scene change
        SceneManager.sceneLoaded += OnSceneLoaded;
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // hapus event listener
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        uiManager = FindFirstObjectByType<UIManager>();
        spawnSystem = FindFirstObjectByType<SpawnSystem>();

        spawnSystem.SpawnAtStart();
    }

    void Start()
    {
        lives = maxLives;
    }

    public void SaveCheckpoint()
    {
        if (!isAtCheckpoint)
        {
            spawnSystem.FirstCheckpoint();
            isAtCheckpoint = true;
        }
        else
            spawnSystem.NextCheckpoint();
    }

    public async void Respawn()
    {
        await Task.Delay(respawnDelay * 1000); // Convert seconds to milliseconds

        if (lives > 1)
        {    
            lives--;
            if (isAtCheckpoint)
            {
                spawnSystem.SpawnAtCheckpoint();
            }
            else
            {
                spawnSystem.SpawnAtStart();
            }
        }
        else // lives == 0
        {
            isAtCheckpoint = false;
            lives = maxLives;
            uiManager.GameOver();
        }

        isHit = false;
    }

    public async void NextLevel()
    {   
        await Task.Delay(1 * 1000);

        currentSceneIndex++;
        if (currentSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            currentSceneIndex = 0; // Kembali ke menu utama atau scene pertama
        }
        SceneManager.LoadScene(currentSceneIndex);
    }
}

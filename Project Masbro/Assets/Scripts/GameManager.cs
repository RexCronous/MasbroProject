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

    [Header("Timer")]
    public float elapsedTime = 0f;
    public float fastestTime = 0f; 
    
    [Header("Level Progression")]
    public int levelUnlocked; 
    public string key; 
    private bool levelFinished = false;

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
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Daftarkan event untuk scene change
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        if (!levelFinished)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // hapus event listener
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Pastikan timeScale normal saat pindah scene
        Time.timeScale = 1f;

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex == 0) // Main Menu
        {
            Destroy(gameObject);
            return;
        }
        
        elapsedTime = 0f;
        levelFinished = false;

        uiManager = FindFirstObjectByType<UIManager>();
        spawnSystem = FindFirstObjectByType<SpawnSystem>();

        lives = maxLives;
        isAtCheckpoint = false;
        isHit = false;

        spawnSystem.SpawnAtStart();
    }

    public void SaveCheckpoint(int Index)
    {
        spawnSystem = FindFirstObjectByType<SpawnSystem>();
        spawnSystem.index = Index;
        isAtCheckpoint = true;
    }

    public void FinishLevel()
    {
        levelFinished = true;

        levelUnlocked = PlayerPrefs.GetInt("LevelUnlocked", 1);
        key = "FastestTime_Level" + currentSceneIndex;
        fastestTime = PlayerPrefs.GetFloat(key, 0f);

        if (currentSceneIndex >= levelUnlocked)
        {
            PlayerPrefs.SetInt("LevelUnlocked", currentSceneIndex + 1);
        }

        // Simpan waktu tercepat baru bila lebih baik
        if (fastestTime == 0f || elapsedTime < fastestTime)
        {
            fastestTime = elapsedTime;
            PlayerPrefs.SetFloat(key, elapsedTime);
        }

        uiManager = uiManager ?? FindFirstObjectByType<UIManager>();
        uiManager?.Finish();
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
            lives = 0;
            uiManager = uiManager ?? FindFirstObjectByType<UIManager>();
            uiManager?.GameOver();
        }

        isHit = false;
    }
}

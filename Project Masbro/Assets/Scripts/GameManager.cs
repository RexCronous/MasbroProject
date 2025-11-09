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
    public int numberOfCheckpoints;

    [Header("Timer")]
    public float elapsedTime = 0f;
    public float fastestTime = 0f;

    // Level Progression
    private int levelUnlocked;
    private int checkpointsReached = 0;
    private string key;
    private bool levelFinished = false;

    // Properties untuk StarBar
    public int CheckpointsReached => checkpointsReached;
    public int TotalCheckpoints => numberOfCheckpoints;

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
            uiManager = uiManager ?? FindFirstObjectByType<UIManager>();
            uiManager?.UpdateProgress(checkpointsReached, numberOfCheckpoints);
            uiManager?.UpdateTimer(elapsedTime);
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

        numberOfCheckpoints = spawnSystem.checkpoint.Length;
        checkpointsReached = 0;

        lives = maxLives;
        isAtCheckpoint = false;
        isHit = false;

        spawnSystem.SpawnAtStart();
    }

    public void SaveCheckpoint(int Index, bool hasActivated)
    {
        spawnSystem = FindFirstObjectByType<SpawnSystem>();
        if (!hasActivated)
        {
            checkpointsReached++;
        }
        spawnSystem.index = Index;
        isAtCheckpoint = true;
    }

    public void FinishLevel()
    {
        levelFinished = true;

        // Simpan progress checkpoint untuk star bar — hanya jika lebih tinggi dari yang tersimpan
        float progress = numberOfCheckpoints > 0 ? (float)checkpointsReached / numberOfCheckpoints : 0f;
        string progressKey = $"Level{currentSceneIndex}_Progress";
        float prevProgress = PlayerPrefs.GetFloat(progressKey, 0f);
        if (progress > prevProgress)
        {
            PlayerPrefs.SetFloat(progressKey, progress);
            Debug.Log($"Updated stored progress for Level {currentSceneIndex}: {prevProgress} -> {progress}");
        }

        // Unlock level berikutnya (set unlock flag)
        int nextLevel = currentSceneIndex + 1;
        PlayerPrefs.SetInt($"Level{nextLevel}_Unlocked", 1);

        // Simpan waktu tercepat
        key = "FastestTime_Level" + currentSceneIndex;
        fastestTime = PlayerPrefs.GetFloat(key, float.MaxValue);
        if (elapsedTime < fastestTime)
        {
            fastestTime = elapsedTime;
            PlayerPrefs.SetFloat(key, elapsedTime);
        }

        // Pastikan perubahan tersimpan
        PlayerPrefs.Save();

        Debug.Log($"Level {currentSceneIndex} selesai - Progress: {progress}, previous: {prevProgress}, Unlocking Level {nextLevel}");

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
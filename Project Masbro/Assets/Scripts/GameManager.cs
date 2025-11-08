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
    [HideInInspector] public bool atTutorial = false;

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

        if (currentSceneIndex == SceneManager.sceneCountInBuildSettings - 3) // Tutorial level
        {
            atTutorial = true;
        }
        else
        {
            atTutorial = false;
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

    public void SaveCheckpoint(int Index)
    {
        spawnSystem = FindFirstObjectByType<SpawnSystem>();
        if (Index > spawnSystem.previousIndex)
        {
            spawnSystem.previousIndex = Index;
            checkpointsReached++;
        }
        spawnSystem.index = Index;
        isAtCheckpoint = true;
    }

    public void FinishLevel()
    {
        levelFinished = true;

        // Simpan progress checkpoint untuk star bar
        float progress = (float)checkpointsReached / numberOfCheckpoints;
        PlayerPrefs.SetFloat($"Level{currentSceneIndex}_Progress", progress);

        // Unlock level berikutnya
        int nextLevel = currentSceneIndex + 1;
        PlayerPrefs.SetInt($"Level{nextLevel}_Unlocked", 1);

        // Simpan waktu tercepat
        key = "FastestTime_Level" + currentSceneIndex;
        fastestTime = PlayerPrefs.GetFloat(key, elapsedTime);

        if (atTutorial) // Tutorial level completed
        {
            print("Tutorial Completed");
            PlayerPrefs.SetInt("Tutorial_Done", 1);
        }
        // else if (currentSceneIndex >= levelUnlocked) // Unlock next level
        // {
        //     PlayerPrefs.SetInt("LevelUnlocked", currentSceneIndex + 1);
        // }

        // Simpan waktu tercepat baru bila lebih baik
        fastestTime = PlayerPrefs.GetFloat(key, float.MaxValue);
        if (elapsedTime < fastestTime)
        {
            fastestTime = elapsedTime;
            PlayerPrefs.SetFloat(key, elapsedTime);
        }

        // Pastikan perubahan tersimpan
        PlayerPrefs.Save();

        Debug.Log($"Level {currentSceneIndex} selesai - Progress: {progress}, Unlocking Level {nextLevel}");

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

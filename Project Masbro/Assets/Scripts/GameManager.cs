using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public SpawnSystem spawnSystem;
    public int maxLives = 3;
    public int lives;
    public int respawnDelay = 1; // in seconds
    public bool isAtCheckpoint = false;
    public bool isHit = false;
    private UIManager uiManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        lives = maxLives;
        spawnSystem.SpawnAtStart();
    }

    private void Awake()
    {
        uiManager = FindFirstObjectByType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {

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
            spawnSystem.SpawnAtStart();
            uiManager.GameOver();
        }

        isHit = false;
    }
}

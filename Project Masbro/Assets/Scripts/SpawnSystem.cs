using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    public static SpawnSystem instance;
    public GameObject objectPrefab;
    public Transform spawnPoint;
    public int lives = 3;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (lives <= 0) return;

        Instantiate(objectPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void Respawn()
    {
        if (lives > 0)
        {
            Invoke(nameof(SpawnPlayer), 1.0f);
            lives--;
        }
    }
}

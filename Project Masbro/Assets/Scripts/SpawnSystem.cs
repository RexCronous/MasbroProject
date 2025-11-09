using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject starting;
    public GameObject[] checkpoint;
    public int index = 0;

    public void SpawnAtStart()
    {
        SpawnPlayer(starting.transform);
    }

    public void SpawnAtCheckpoint()
    {
        SpawnPlayer(checkpoint[index].transform);
    }

    private void SpawnPlayer(Transform spawnPoint)
    {
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}

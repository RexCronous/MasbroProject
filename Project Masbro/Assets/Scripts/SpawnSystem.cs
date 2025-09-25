using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform startingPosition;
    public Transform checkpointPosition;

    public void SpawnAtStart()
    {
        SpawnPlayer(startingPosition);
    }

    public void SpawnAtCheckpoint()
    {
        SpawnPlayer(checkpointPosition);
    }

    private void SpawnPlayer(Transform spawnPoint)
    {
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}

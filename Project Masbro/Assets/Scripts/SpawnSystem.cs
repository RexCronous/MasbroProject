using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform startingPosition;
    public Transform[] checkpointPosition;
    private int index = 0;


    public void SpawnAtStart()
    {
        SpawnPlayer(startingPosition);
    }

    public void SpawnAtCheckpoint()
    {
        SpawnPlayer(checkpointPosition[Mathf.Clamp(index, 0, checkpointPosition.Length - 1)]);
        index++;
    }

    private void SpawnPlayer(Transform spawnPoint)
    {
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}

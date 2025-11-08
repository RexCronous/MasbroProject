using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject starting;
    public GameObject[] checkpoint;
    public int index = 0;
    public int previousIndex = -1;

    private void Awake()
    {
        for (int i = 0; i < checkpoint.Length; i++)
        {
            var cp = checkpoint[i].GetComponent<Checkpoint>();
            cp.SetIndex(i);
        }
    }

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

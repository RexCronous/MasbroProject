using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject starting;
    public GameObject[] checkpoint;
    private int index = 0;


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
    
    public void NextCheckpoint()
    {
        NextIndex();
        checkpoint[index].SetActive(false);
    }

    public void FirstCheckpoint()
    {
        checkpoint[index].SetActive(false);
    }

    private void NextIndex()
    {
        if (index < checkpoint.Length - 1)
        {
            index++;
        }
        else
        {
            return;
        }
    }
}

using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool colliderState = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            colliderState = false;
            GetComponent<Collider2D>().enabled = colliderState;   
            GameManager.Instance.SaveCheckpoint();
        }
    }
}

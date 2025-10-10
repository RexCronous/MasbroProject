using UnityEngine;

public class Objects : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            if (!GameManager.Instance.isHit)
            {
                Destroy(other.gameObject);
                GameManager.Instance.Respawn();
                GameManager.Instance.isHit = true;
            }
        }
    }
}

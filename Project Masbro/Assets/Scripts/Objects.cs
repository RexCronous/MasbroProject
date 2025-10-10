using UnityEngine;

public class Objects : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

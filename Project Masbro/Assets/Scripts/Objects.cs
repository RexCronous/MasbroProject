using UnityEngine;

public class Objects : MonoBehaviour
{
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!GameManager.Instance.isHit)
            {
                // Play damage sound immediately when hit
                if (audioManager != null && audioManager.takeDamage != null)
                {
                    audioManager.PlaySfx(audioManager.takeDamage);
                }

                Destroy(other.gameObject);
                GameManager.Instance.Respawn();
                GameManager.Instance.isHit = true;
            }
        }
    }
}

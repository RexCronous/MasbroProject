using System.Threading.Tasks;
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

                ParticleSystem deathFx = other.gameObject.GetComponent<PlayerController>().DeathFX;
                ParticleSystem deathObject = Instantiate(deathFx, other.transform.position, Quaternion.identity);
                
                Destroy(other.gameObject);
                GameManager.Instance.Respawn();
                GameManager.Instance.isHit = true;
                
                DestroyAfterDelay(deathObject.gameObject, 1000);
            }
        }
    }

    private async void DestroyAfterDelay(GameObject obj, int delay)
    {
        await Task.Delay(delay);
        Destroy(obj);
    }
}
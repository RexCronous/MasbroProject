using UnityEngine;

public class InteractPlayer : MonoBehaviour
{
    private AudioManager audioManager;
    //private float lastPlayTime = 0f; 
    //public float minInterval = 0.2f; // jeda antar suara biar ga terlalu sering

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Pastikan hanya memutar suara jika benar-benar menyentuh objek dengan tag tertentu
        if (collision.collider.CompareTag("Object") && audioManager != null && audioManager.boxTouch != null)
        {
            if (audioManager != null && audioManager.boxTouch != null)
            {
                audioManager.PlaySfx(audioManager.boxTouch);
            }
            // // Cegah suara berulang terlalu cepat (misalnya saat banyak box disentuh cepat)
            // if (Time.time - lastPlayTime > minInterval)
            // {
            //     lastPlayTime = Time.time;

            //     // Randomisasi sedikit volume & pitch biar kesannya natural
            //     float randomPitch = Random.Range(0.9f, 1.1f);
            //     float randomVolume = Random.Range(0.85f, 1f);

            //     audioManager.PlaySfx(audioManager.boxTouch, randomVolume, randomPitch);
            // }
        }
    }
}

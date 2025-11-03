using UnityEngine;
using UnityEngine.UI;


public class SelectionArrow : MonoBehaviour
{
    private RectTransform rect;
    [SerializeField] private RectTransform[] options;       // Daftar posisi menu
    [SerializeField] private AudioClip changeSound;         // Suara saat ganti pilihan
    [SerializeField] private AudioClip interactSound;       // Suara saat pilih menu
    private AudioSource audioSource;
    private int currentPosition = 0;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            // Jika belum ada AudioSource, tambahkan otomatis
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        // Navigasi ke atas
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            ChangePosition(-1);

        // Navigasi ke bawah
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            ChangePosition(1);

        // Konfirmasi (misalnya tombol Enter / Space)
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            Interact();
    }

    private void ChangePosition(int change)
    {
        if (options.Length == 0) return;

        currentPosition += change;

        // Putar suara hanya kalau benar-benar berubah
        if (change != 0 && changeSound != null)
            audioSource.PlayOneShot(changeSound);

        // Looping posisi (atas-bawah)
        if (currentPosition < 0)
            currentPosition = options.Length - 1;
        else if (currentPosition >= options.Length)
            currentPosition = 0;

        // Pindahkan posisi panah
        rect.position = new Vector3(rect.position.x, options[currentPosition].position.y, rect.position.z);
    }

    private void Interact()
    {
        if (interactSound != null)
            audioSource.PlayOneShot(interactSound);

        // Tambahkan aksi sesuai menu yang dipilih
        Debug.Log($"Selected option index: {currentPosition}");
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
    }

}

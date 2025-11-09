using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("==========Audio Source==========")]
    [SerializeField] AudioSource musicSource; // untuk soundtrack
    [SerializeField] AudioSource sfxSource; // untuk semua sfx normal
    [SerializeField] AudioSource footstepSource; // untuk footstep (punya pitch sendiri)

    [Header("==========Audio Clip==========")]
    public AudioClip musicIdle;

    [Header("==========Audio Clip==========")]
    public AudioClip gameOver; //done
    public AudioClip finish;
    public AudioClip selectItemGameOverMenu; // done
    public AudioClip interactItemGameOverMenu; // done
    public AudioClip takeDamage; // done
    public AudioClip jump; // done
    public AudioClip boxTouch; // done
    public AudioClip[] walking; // done
    public AudioClip checkPoint; // done
    public AudioClip openPause; // done
    public AudioClip closedPause; // done
    public AudioClip finished; // done

    [Header("==========Pitch Setting==========")]
    [Range(0.1f, 3f)]
    public float walkPitch = 1.0f;

    [Header("==========Fade Settings==========")]
    public float fadeDuration = 0f;


    // music is ready
    // start music (when music is ready)
    private void Start()
    {
        // mengatur klip dan pastikan looping dimatikan (loop secara manual)
        musicSource.clip = musicIdle;
        musicSource.loop = false;

        // mulai fade in
        StartCoroutine(FadeIn(musicSource, fadeDuration));
    }


    // dipanggil ketika music selesai
    private void Update()
    {
        // cek jika musik telah berhenti
        if (!musicSource.isPlaying && musicSource.clip != null)
        {
            // muali proses fade out
            StartCoroutine(FadeOutAndIn(musicSource, fadeDuration));
        }
    }

    IEnumerator FadeIn(AudioSource audioSource, float duration)
    {
        // Atur volume awal ke 0 dan Mulai Play()
        audioSource.volume = 0f;
        audioSource.Play();

        float startVolume = 0f;
        float targetVolume = 1f;
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    IEnumerator FadeOutAndIn(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;
        float targetVolume = 0f;
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;

        audioSource.time = 0f;
        audioSource.Play();

        yield return StartCoroutine(FadeIn(audioSource, duration));
    }

    private IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;
        float targetVolume = 0f;
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
            yield return null;
        }

        audioSource.volume = 0f;
    }

    public IEnumerator FadeOutMusic()
    {
        yield return StartCoroutine(FadeOut(musicSource, fadeDuration));
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
        // print("Play SFX: " + clip.name);
    }

    public void PlayFootstep(AudioClip clip)
    {
        footstepSource.PlayOneShot(clip);
    }

    public void SetFootstepPitchWalk()
    {
        footstepSource.pitch = walkPitch;
    }
}

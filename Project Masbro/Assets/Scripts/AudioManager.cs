using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("========== Audio Mixer ==========")]
    [SerializeField] private AudioMixer mixer;

    [Header("========== Audio Source ==========")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource footstepSource;

    [Header("========== Audio Clip ==========")]
    public AudioClip musicIdle;

    public AudioClip gameOver;
    public AudioClip finish;
    public AudioClip selectItemGameOverMenu;
    public AudioClip interactItemGameOverMenu;
    public AudioClip takeDamage;
    public AudioClip jump;
    public AudioClip boxTouch;
    public AudioClip[] walking;
    public AudioClip checkPoint;
    public AudioClip openPause;
    public AudioClip closedPause;
    public AudioClip finished;

    [Header("========== Pitch Setting ==========")]
    [Range(0.1f, 3f)]
    public float walkPitch = 1.0f;

    [Header("========== Fade Settings ==========")]
    private float fadeDuration = 0.5f;

    private void Start()
    {
        // 1. Baca volume dari PlayerPrefs
        float musicVol = PlayerPrefs.GetFloat("musicVolume", 1f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);

        ApplyMixerVolume(musicVol, sfxVol);

        // 2. Setup musik idle
        musicSource.clip = musicIdle;
        musicSource.loop = false;
        musicSource.volume = 1f;

        // Volume audioSource = 0 untuk fade in (Mixer tetap pakai nilai prefs)
        musicSource.volume = 0f;
        StartCoroutine(FadeIn(musicSource, fadeDuration));
    }

    private void ApplyMixerVolume(float music, float sfx)
    {
        mixer.SetFloat("music", Mathf.Log10(music) * 20);
        mixer.SetFloat("SFX", Mathf.Log10(sfx) * 20);
    }

    private void Update()
    {
        if (!musicSource.isPlaying && musicSource.clip != null)
        {
            StartCoroutine(FadeOutAndIn(musicSource, fadeDuration));
        }
    }

    IEnumerator FadeIn(AudioSource source, float duration)
    {
        float t = 0f;
        source.Play();

        while (t < duration)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }

        source.volume = 1f;
    }

    IEnumerator FadeOutAndIn(AudioSource source, float duration)
    {
        float start = source.volume;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(start, 0f, t / duration);
            yield return null;
        }

        source.volume = 0f;
        source.time = 0f;
        source.Play();

        yield return StartCoroutine(FadeIn(source, duration));
    }

    public IEnumerator FadeOutMusic()
    {
        yield return StartCoroutine(FadeOut(musicSource, fadeDuration));
    }

    private IEnumerator FadeOut(AudioSource source, float duration)
    {
        float start = source.volume;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(start, 0f, t / duration);
            yield return null;
        }

        source.volume = 0f;
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayFootstep(AudioClip clip)
    {
        footstepSource.pitch = walkPitch;
        footstepSource.PlayOneShot(clip);
    }

    public void SetFootstepPitchWalk()
    {
        footstepSource.pitch = walkPitch;
    }

}

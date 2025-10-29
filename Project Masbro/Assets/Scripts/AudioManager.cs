using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("==========Audio Source==========")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("==========Audio Clip==========")]
    public AudioClip gameOver; //done
    public AudioClip selectItemGameOverMenu; // done
    public AudioClip interactItemGameOverMenu; // not yet
    public AudioClip takeDamage; // done
    public AudioClip jump; // done
    public AudioClip boxTouch; // not yet
    public AudioClip walking; // ambigous
    public AudioClip run; // same ambigous

    // start music (when music is ready)
    // private void Start()
    // {

    // }

    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("==========Audio Source==========")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("==========Audio Clip==========")]
    public AudioClip gameOver; //done
    public AudioClip selectItemGameOverMenu; // done
    public AudioClip interactItemGameOverMenu; // done
    public AudioClip takeDamage; // done
    public AudioClip jump; // done
    public AudioClip boxTouch; // not yet
    public AudioClip[] walking; // done
    public AudioClip run; // not yet
    public AudioClip checkPoint; // not yet
    public AudioClip openPause; // done
    public AudioClip closedPause; // done

    // music is ready

    // start music (when music is ready)
    // private void Start()
    // {

    // }

    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}

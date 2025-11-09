using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("==========Audio Source==========")]
    [SerializeField] AudioSource musicSource; // untuk soundtrack
    [SerializeField] AudioSource sfxSource;  // untuk semua sfx normal
    [SerializeField] AudioSource footstepSource;     // untuk footstep (punya pitch sendiri)

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

    [Header("==========Pitch Setting==========")]
    [Range(0.1f, 3f)]
    public float walkPitch = 1.0f;


    // music is ready

    // start music (when music is ready)
    // private void Start()
    // {

    // }

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

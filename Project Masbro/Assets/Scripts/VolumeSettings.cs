using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void OnEnable()
    {
        LoadVolume();
    }

    public void SetMusicVolume()
    {
        float v = musicSlider.value;
        mixer.SetFloat("music", Mathf.Log10(v) * 20);
        PlayerPrefs.SetFloat("musicVolume", v);
    }

    public void SetSFXVolume()
    {
        float v = sfxSlider.value;
        mixer.SetFloat("SFX", Mathf.Log10(v) * 20);
        PlayerPrefs.SetFloat("SFXVolume", v);
    }

    public void LoadVolume()
    {
        float mv = PlayerPrefs.GetFloat("musicVolume", 1f);
        float sv = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSlider.value = mv;
        sfxSlider.value = sv;

        SetMusicVolume();
        SetSFXVolume();
    }
}

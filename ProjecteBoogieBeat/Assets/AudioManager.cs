using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioManager
{
    [SerializeField] private static AudioSource SFX_AudioSource;
    [SerializeField] private static AudioSource OST_AudioSource;

    static float SFX_Volume = 1.0f;
    static float OST_Volume = 1.0f;

    public static void InitAudioManager()
    {
        //SFX_Volume = PlayerPrefs.GetFloat("sfxVolume", 1.0f);
        //OST_Volume = PlayerPrefs.GetFloat("ostVolume", 1.0f);

        SFX_AudioSource.volume = SFX_Volume;
        OST_AudioSource.volume = OST_Volume;

    }


    public static void PlaySFX(string _audioName)
    {
        AudioClip clip = Resources.Load<AudioClip>("/Audios/SFX/" + _audioName);
        SFX_AudioSource.PlayOneShot(clip);
    }

    public static void PlayOST(string _audioName)
    {
        AudioClip clip = Resources.Load<AudioClip>("/Audios/OST/" + _audioName);
        OST_AudioSource.clip = clip;
        OST_AudioSource.Play();
    }

}

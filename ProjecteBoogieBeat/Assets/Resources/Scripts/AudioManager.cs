using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] float SFX_Volume = 1.0f;
    [SerializeField] [Range(0, 1)] float OST_Volume = 1.0f;
    
    private static AudioSource SFX_AudioSource;
    private static AudioSource OST_AudioSource;


    public void Start()
    {
        SFX_AudioSource = transform.GetChild(0).GetComponent<AudioSource>();
        OST_AudioSource = transform.GetChild(1).GetComponent<AudioSource>();

        //SFX_Volume = PlayerPrefs.GetFloat("sfxVolume", 1.0f);
        //OST_Volume = PlayerPrefs.GetFloat("ostVolume", 1.0f);

        SFX_AudioSource.volume = SFX_Volume;
        OST_AudioSource.volume = OST_Volume;

    }


    public static void Play_SFX(string _audioName)
    {
        AudioClip clip = Resources.Load<AudioClip>("/Audio/SFX/" + _audioName);
        SFX_AudioSource.PlayOneShot(clip);
    }

    public static void Play_OST(string _audioName)
    {
        AudioClip clip = Resources.Load<AudioClip>("/Audio/OST/" + _audioName);
        OST_AudioSource.clip = clip;
        OST_AudioSource.Play();
    }
    public static void RePlay_OST()
    {
        OST_AudioSource.Play();
    }


    public static bool OST_IsPlaying()
    {
        return OST_AudioSource.isPlaying;
    }

}

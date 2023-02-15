using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MenuSoundManager : MonoBehaviour
{
    public static MenuSoundManager instance;
    public AudioSource musicSource;
    public AudioSource effectSource;
    public AudioClip menuMusic;
    public AudioClip menuAccept;
    public AudioClip menuOpen;
    public AudioClip menuCancel;
    public AudioClip levelUp;

    public void Awake()
    {
        instance = this;
    }
    
    public void PlayOpen()
    {
        effectSource.PlayOneShot(menuOpen);
    }
    
    public void PlayAccept()
    {
        effectSource.PlayOneShot(menuAccept);
    }
    
    public void PlayCancel()
    {
        effectSource.PlayOneShot(menuCancel);
    }
    
    public void PlayMusic()
    {
        musicSource.clip = menuMusic;
        musicSource.Play();
    }
    
    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void UpdateVolume()
    {
        effectSource.volume = PlayerPrefs.GetFloat("effectVolume", 1);
        musicSource.volume = PlayerPrefs.GetFloat("musicVolume", 1);
    }

    public void PlayLevelUp()
    {
        effectSource.PlayOneShot(levelUp);
    }
}

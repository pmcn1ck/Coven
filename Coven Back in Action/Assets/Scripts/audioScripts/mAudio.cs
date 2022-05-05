using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eSound
{
    click,
    goBack,
    hover

}

public enum eMusic
{
    MainMenu,
    OverWorld,
    Combat,
    FinalCombat
}

public class mAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource musicSource;
    public AudioClip[] soundFiles;
    public AudioClip[] musicFiles;

    public void PlaySound(eSound _soundIndex)
    {
        audioSource.PlayOneShot(soundFiles[(int)_soundIndex]);
    }

    public void StopSound(eSound _soundIndex)
    {
        audioSource.Stop();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void PlayMusic(eMusic _soundIndex)
    {
        musicSource.PlayOneShot(musicFiles[(int)_soundIndex]);
    }
}

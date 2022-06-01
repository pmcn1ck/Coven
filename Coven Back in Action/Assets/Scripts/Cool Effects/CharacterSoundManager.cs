using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundManager : MonoBehaviour
{
    public AudioSource SoundEffects;
    public AudioClip attack;
    public AudioClip damage;
    public List<AudioClip> extraSounds;

    public void CallAttackAudio()
    {
        SoundEffects.PlayOneShot(attack);
    }
    
    public void CallDamageAudio()
    {
        SoundEffects.PlayOneShot(damage);
    }

    public void CallExtraAudio(int x)
    {
        SoundEffects.PlayOneShot(extraSounds[x]);
    }


}

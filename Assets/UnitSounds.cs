using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSounds : MonoBehaviour
{
    /*[Header("Voice Overs")]
    public List<AudioClip> selectedVO;
    public List<AudioClip> attackingVO;
    public List<AudioClip> specialVO;
    public List<AudioClip> hurtVO;*/

    [Header("SFXs")]
    public AudioClip attackClip;
    public AudioClip specialClip;
    public AudioClip specialClipPart2;
    public AudioClip hitClip;

    public AudioSource unitAudioSource;
    public AudioSource unitSFXAudioSource;

    void PlayHitSound()
    {
        unitSFXAudioSource.clip = hitClip;
        
        /*AudioClip hurtClip = hurtVO[Random.Range(0, hurtVO.Count)];
        unitAudioSource.clip = hurtClip;*/

        unitSFXAudioSource.Play();
        unitAudioSource.Play();
    }

    void PlaySpecialVO()
    {
        //unitAudioSource.clip = specialVO[Random.Range(0, specialVO.Count)];

        unitAudioSource.Play();
    }

    void PlaySpecialSound()
    {
        unitSFXAudioSource.clip = specialClip;

        unitSFXAudioSource.Play();
    }

    void PlayAttacklSound()
    {
        unitSFXAudioSource.clip = attackClip;

        unitSFXAudioSource.Play();
    }

    void PlaySpecialSoundTwo()
    {
        unitSFXAudioSource.clip = specialClipPart2;

        unitSFXAudioSource.Play();
    }
}

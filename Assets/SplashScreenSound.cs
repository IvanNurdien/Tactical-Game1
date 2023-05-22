using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenSound : MonoBehaviour
{
    public AudioClip splashSound;

    public AudioSource splashSoundSource;

    void playSFX()
    {
        splashSoundSource.clip = splashSound;

        splashSoundSource.Play();
    }
    

}

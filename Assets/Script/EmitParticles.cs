using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitParticles : MonoBehaviour
{
    public ParticleSystem particles;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayParticle()
    {
        if (!particles.isPlaying)
        {
            particles.Play();
            Debug.Log("I shoot imaginary cards!");
       }
    }

    public void StopParticle()
    {
        particles.Stop();
    }
}

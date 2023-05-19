using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitParticles : MonoBehaviour
{
    public ParticleSystem particles;
    public ParticleSystem particlesOnTarget;
    MovementScript ms;
    GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        ms = GetComponent<MovementScript>();
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

    public void PlayMageSpecialParticle()
    {
        target = ms.target;

        particlesOnTarget.gameObject.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 40, target.transform.position.z);

        particlesOnTarget.Play();
    }

    public void StopParticle()
    {
        particles.Stop();
    }
}

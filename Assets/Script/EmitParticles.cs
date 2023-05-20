using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitParticles : MonoBehaviour
{
    public ParticleSystem particles;
    public ParticleSystem secondParticles;
    public ParticleSystem particlesOnTarget;
    MovementScript ms;
    GameObject target;

    private const byte SYNC_PARTICLES = 12;

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == SYNC_PARTICLES)
        {
            object[] datas = (object[])obj.CustomData;
            float unitViewID = (float)datas[0];

            float posX = (float)datas[1];
            float posY = (float)datas[2];
            float posZ = (float)datas[3];

            if (GetComponentInParent<PhotonView>().ViewID == unitViewID && target == null)
            {
                Debug.Log("Particles is PARTICLESD");

                particlesOnTarget.gameObject.transform.position = new Vector3(posX, posY, posZ);
            }
        }
    }


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

    public void PlaySecondParticle()
    {
        if (!secondParticles.isPlaying)
        {
            secondParticles.Play();
        }
    }

    public void PlayMageSpecialParticle()
    {
        target = ms.target;

        particlesOnTarget.Play();

        if (target != null)
        {
            particlesOnTarget.gameObject.transform.position = target.transform.position;

            float thisUnitViewID = GetComponentInParent<PhotonView>().ViewID;
            float posX = target.transform.position.x;
            float posY = target.transform.position.y;
            float posZ = target.transform.position.z;
            object[] datas = new object[] { thisUnitViewID, posX, posY, posZ };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(SYNC_PARTICLES, datas, raiseEventOptions, SendOptions.SendReliable);
        }
    }

    public void StopParticle()
    {
        particles.Stop();
    }
}

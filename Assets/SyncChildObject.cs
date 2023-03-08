using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncChildObject : MonoBehaviourPun, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                if (this.transform.GetChild(i) != null)
                {
                    stream.SendNext(this.transform.GetChild(i).localPosition);
                    stream.SendNext(this.transform.GetChild(i).localRotation);
                    stream.SendNext(this.transform.GetChild(i).localScale);
                }
            }
        } else if (stream.IsReading)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                if (this.transform.GetChild(i) != null)
                {
                    this.transform.GetChild(i).localPosition = (Vector3)stream.ReceiveNext();
                    this.transform.GetChild(i).localPosition = (Vector3)stream.ReceiveNext();
                    this.transform.GetChild(i).localScale = (Vector3)stream.ReceiveNext();
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

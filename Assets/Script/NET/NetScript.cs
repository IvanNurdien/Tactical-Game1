 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetScript : MonoBehaviour
{
    public PhotonView playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        //conecting
        PhotonNetwork.ConnectUsingSettings();
    }
    public void OnConnectedToMaster()
    {
        //kita terhubung
        Debug.Log("konek ke master");
        PhotonNetwork.JoinRandomOrCreateRoom();
    }
    public  void OnJoinedRoom()
    {
        Debug.Log("join sukses");
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);

    }
}

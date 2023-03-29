 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class NetScript : MonoBehaviourPunCallbacks
{
    public PhotonView playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        //connecting
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("conecting");
    }

    public override void OnConnectedToMaster()
    {
        //kita terhubung
        Debug.Log("konek ke master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("join sukses");
        SceneManager.LoadScene("Lobby");
        
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);    
        Debug.Log("join sukses");
    }
}

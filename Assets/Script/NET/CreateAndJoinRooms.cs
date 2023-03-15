using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;


    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        /*roomOptions.MaxPlayers = 2;
        roomOptions.BroadcastPropsChangeToAll = true;*/
        PhotonNetwork.CreateRoom(createInput.text, new RoomOptions() { MaxPlayers = 2, BroadcastPropsChangeToAll = true });
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Unit Select Screen");
    }
}

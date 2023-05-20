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
    public TMP_InputField usernameInput;
    public GameObject usernameWarning;

    public void CreateRoom()
    {
        if (usernameInput.text.Length >= 1)
        {
            PhotonNetwork.NickName = usernameInput.text;

            RoomOptions roomOptions = new RoomOptions();
            /*roomOptions.MaxPlayers = 2;
            roomOptions.BroadcastPropsChangeToAll = true;*/
            PhotonNetwork.CreateRoom(createInput.text, new RoomOptions() { MaxPlayers = 2, BroadcastPropsChangeToAll = true });
        } else
        {
            usernameWarning.SetActive(true);
        }

        
    }

    public void JoinRoom()
    {
        if (usernameInput.text.Length >= 1)
        {
            PhotonNetwork.NickName = usernameInput.text;

            PhotonNetwork.JoinRoom(joinInput.text);
        } else
        {
            usernameWarning.SetActive(true);
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Unit Select Screen");
    }
}

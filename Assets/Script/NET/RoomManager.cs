using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public List<PlayerItem> playerItemsList = new List<PlayerItem>();
    public PlayerItem playerItemPrefab;
    public Transform player1Position;
    public Transform player2Position;
    public GameObject startButton;

    public bool player1Ready;
    public bool player2Ready;

    public List<GameObject> unitButtons;

    // Start is called before the first frame update
    void Start()
    {
        UpdatePlayerList();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            foreach (PlayerItem item in playerItemsList)
            {
                if (item.thisPlayer == PhotonNetwork.LocalPlayer)
                {
                    item.UnselectLocalUnits();
                }
            }
        }


        // ACTIVATE THIS WHEN DONE  
        /*if (PhotonNetwork.LocalPlayer == PhotonNetwork.MasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            if (player1Ready && player2Ready)
            {
                startButton.SetActive(true);
            }
            else
            {
                startButton.SetActive(false);
            }
        } else
        {
            startButton.SetActive(false);
        }*/
    }

    // UPDATE PLAYER IN ROOM
    void UpdatePlayerList()
    {
        foreach (PlayerItem item in playerItemsList)
        {
            Destroy(item.gameObject);
        }
        playerItemsList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value == PhotonNetwork.MasterClient)
            {
                PlayerItem newPlayerItem = Instantiate(playerItemPrefab, player1Position);
                newPlayerItem.SetPlayerInfo(player.Value);
                playerItemsList.Add(newPlayerItem);
            } else
            {
                PlayerItem newPlayerItem = Instantiate(playerItemPrefab, player2Position);
                newPlayerItem.SetPlayerInfo(player.Value);
                playerItemsList.Add(newPlayerItem);
            }
        }
    }

    // TO SHOW PLAYER SYMBOL ON UNIT BUTTON
    public void SelectedUnit(Unit units)
    {
        foreach(PlayerItem item in playerItemsList)
        {
            if (item.thisPlayer == PhotonNetwork.LocalPlayer)
            {
                GameObject button = EventSystem.current.currentSelectedGameObject.gameObject;
                item.ApplyLocalChanges(units, button);
                PhotonView buttonPV = button.GetComponent<PhotonView>();
                int buttonID = button.GetComponent<UnitButton>().units.unitNumber;

                //button.transform.Find("Player1Chosen").gameObject.SetActive(true);
                //button.GetComponent<UnitButton>().SelectedByOtherPlayer(true);
            }
        }
    }

    public void OnClickPlay()
    {
        PhotonNetwork.LoadLevel("Gameplay 4");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }
}

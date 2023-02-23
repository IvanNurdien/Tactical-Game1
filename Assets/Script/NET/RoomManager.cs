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
    public Transform playerItemParent;

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
    }

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
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
            newPlayerItem.SetPlayerInfo(player.Value);
            playerItemsList.Add(newPlayerItem);
        }
    }

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

                button.transform.Find("Player1Chosen").gameObject.SetActive(true);
                button.GetComponent<UnitButton>().SelectedByOtherPlayer(true);
            }
        }
    }

    /*[PunRPC]
    void RPC_CharSelectedByOtherPlayer(int buttonID)
    {
        foreach(GameObject button in unitButtons)
        {
            if (button.GetComponent<UnitButton>().units.unitNumber == buttonID)
            {
                button.transform.Find("Player2Chosen").gameObject.SetActive(true);
                
            }
        }
    }*/

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    [Header("First Unit")]
    public int firstUnit;
    public TMP_Text firstUnitName;
    GameObject firstUnitButton;

    [Header("Second Unit")]
    public int secondUnit;
    public TMP_Text secondUnitName;
    GameObject secondUnitButton;


    [Header("Third Unit")]
    public int thirdUnit;
    public TMP_Text thirdUnitName;
    GameObject thirdUnitButton;

    [Header("Initialize Player")]
    public Player thisPlayer;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

    public void SetPlayerInfo(Player _player)
    {
        thisPlayer = _player;
        UpdatePlayerItem(thisPlayer);
    }

    // WHEN SELECTING UNITS
    public void ApplyLocalChanges(Unit units, GameObject button)
    {
        if (firstUnit == 0)
        {
            playerProperties["firstUnit"] = units.unitNumber;
            playerProperties["firstUnitName"] = units.name;
            firstUnitButton = button;

        } else if (secondUnit == 0)
        {
            playerProperties["secondUnit"] = units.unitNumber;
            playerProperties["secondUnitName"] = units.name;
            secondUnitButton = button;
            
        } else if (thirdUnit == 0)
        {
            playerProperties["thirdUnit"] = units.unitNumber;
            playerProperties["thirdUnitName"] = units.name;
            thirdUnitButton = button;
        }
        
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    // WHEN DESELECTING UNITS
    public void UnselectLocalUnits()
    {
        if (thirdUnit != 0)
        {
            playerProperties["thirdUnit"] = 0;
            playerProperties["thirdUnitName"] = "";
            thirdUnitButton.GetComponent<UnitButton>().SelectedByOtherPlayer(false);
            thirdUnitButton.transform.Find("Player1Chosen").gameObject.SetActive(false);
            thirdUnitButton = null;

        } else if (secondUnit != 0)
        {
            playerProperties["secondUnit"] = 0;
            playerProperties["secondUnitName"] = "";
            secondUnitButton.GetComponent<UnitButton>().SelectedByOtherPlayer(false);
            secondUnitButton.transform.Find("Player1Chosen").gameObject.SetActive(false);
            secondUnitButton = null;

        }
        else
        {
            playerProperties["firstUnit"] = 0;
            playerProperties["firstUnitName"] = "";
            firstUnitButton.GetComponent<UnitButton>().SelectedByOtherPlayer(false);
            firstUnitButton.transform.Find("Player1Chosen").gameObject.SetActive(false);
            firstUnitButton = null;
        }

        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (thisPlayer == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }
    }

    void UpdatePlayerItem(Player player)
    {
        if (player.CustomProperties.ContainsKey("unitName"))
        {
            // FOR UPDATING PLAYER UNIT SELECTION
            firstUnit = (int)player.CustomProperties["firstUnit"];
            firstUnitName.text = (string)player.CustomProperties["firstUnitName"];
            playerProperties["firstUnit"] = (int)player.CustomProperties["firstUnit"];
            playerProperties["firstUnitName"] = (string)player.CustomProperties["firstUnitName"];

            secondUnit = (int)player.CustomProperties["secondUnit"];
            secondUnitName.text = (string)player.CustomProperties["secondUnitName"];
            playerProperties["secondUnit"] = (int)player.CustomProperties["secondUnit"];
            playerProperties["secondUnitName"] = (string)player.CustomProperties["secondUnitName"];

            thirdUnit = (int)player.CustomProperties["thirdUnit"];
            thirdUnitName.text = (string)player.CustomProperties["thirdUnitName"];
            playerProperties["thirdUnit"] = (int)player.CustomProperties["thirdUnit"];
            playerProperties["thirdUnitName"] = (string)player.CustomProperties["thirdUnitName"];

            // TO UPDATE PLAYER 1 AND 2 READY
            RoomManager RM = GameObject.Find("RoomManager").GetComponent<RoomManager>();
            if (firstUnit != 0 && secondUnit != 0 && thirdUnit != 0)
            {
                if (thisPlayer == PhotonNetwork.MasterClient)
                {
                    RM.player1Ready = true;
                } else
                {
                    RM.player2Ready = true;
                }
            }
            else
            {
                if (thisPlayer == PhotonNetwork.MasterClient)
                {
                    RM.player1Ready = false;
                }
                else
                {
                    RM.player2Ready = false;
                }
            }
        }
        else
        {
            playerProperties["unitName"] = "Not Chosen";
        }
    }
}

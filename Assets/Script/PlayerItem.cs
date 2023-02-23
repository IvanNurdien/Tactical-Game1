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
    Button firstUnitButton;

    [Header("Second Unit")]
    public int secondUnit;
    public TMP_Text secondUnitName;
    Button secondUnitButton;


    [Header("Third Unit")]
    public int thirdUnit;
    public TMP_Text thirdUnitName;
    Button thirdUnitButton;


    [Header("Fourth Unit")]
    public int fourthUnit;
    public TMP_Text fourthUnitName;
    Button fourthUnitButton;


    [Header("Initialize Player")]
    public Player thisPlayer;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

    public void SetPlayerInfo(Player _player)
    {
        thisPlayer = _player;
        UpdatePlayerItem(thisPlayer);
    }

    public void ApplyLocalChanges(Unit units)
    {
        if (firstUnit == 0)
        {
            playerProperties["firstUnit"] = units.unitNumber;
            playerProperties["firstUnitName"] = units.name;
            

        } else if (secondUnit == 0)
        {
            playerProperties["secondUnit"] = units.unitNumber;
            playerProperties["secondUnitName"] = units.name;
            
        } else if (thirdUnit == 0)
        {
            playerProperties["thirdUnit"] = units.unitNumber;
            playerProperties["thirdUnitName"] = units.name;
            
        }
        else
        {
            playerProperties["fourthUnit"] = units.unitNumber;
            playerProperties["fourthUnitName"] = units.name;
            
        }
        
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void UnselectLocalUnits()
    {
        if (fourthUnit != 0)
        {
            playerProperties["fourthUnit"] = 0;
            playerProperties["fourthUnitName"] = "";
        } else if (thirdUnit != 0)
        {
            playerProperties["thirdUnit"] = 0;
            playerProperties["thirdUnitName"] = "";
        } else if (secondUnit != 0)
        {
            playerProperties["secondUnit"] = 0;
            playerProperties["secondUnitName"] = "";
        }
        else
        {
            playerProperties["firstUnit"] = 0;
            playerProperties["firstUnitName"] = "";
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
            
                fourthUnit = (int)player.CustomProperties["fourthUnit"];
                fourthUnitName.text = (string)player.CustomProperties["fourthUnitName"];
                playerProperties["fourthUnit"] = (int)player.CustomProperties["fourthUnit"];
                playerProperties["fourthUnitName"] = (string)player.CustomProperties["fourthUnitName"];
            
        }
        else
        {
            playerProperties["unitName"] = "Not Chosen";
        }
    }
}

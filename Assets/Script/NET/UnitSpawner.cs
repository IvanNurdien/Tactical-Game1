using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class UnitSpawner : MonoBehaviourPunCallbacks
{
    PlayerController pcOne;
    PlayerController pcTwo;

    public GameObject player1Spawn;
    public GameObject player2Spawn;

    Transform firstUnitSpawn;
    Transform secondUnitSpawn;
    Transform thirdUnitSpawn;

    public Transform pcSpawnOne;
    public Transform pcSpawnTwo;

    public List<Unit> unitList;

    public BattleDirector bd;
    public PlayerController playerControllerPrefab;

    

    private void Start()
    {
        UpdatePlayerList();
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnUnits(player1Spawn, pcOne, true);
        } else
        {
            SpawnUnits(player2Spawn, pcTwo, false);
        }
    }

    public void SpawnUnits(GameObject playerSpawn, PlayerController pc, bool isPlayerOne)
    {
            firstUnitSpawn = playerSpawn.transform.Find("Unit 1");
            secondUnitSpawn = playerSpawn.transform.Find("Unit 2");
            thirdUnitSpawn = playerSpawn.transform.Find("Unit 3");
        

        foreach (var unit in unitList)
        {
            if ((int)PhotonNetwork.LocalPlayer.CustomProperties["firstUnit"] == unit.unitNumber)
            {
                GameObject unitToSpawn = unit.characterPrefab;
                Sprite unitSprite = unit.gameplaySprite;
                float unitHealth = unit.maxHP;
                GameObject spawnedUnit = PhotonNetwork.Instantiate(unitToSpawn.name, firstUnitSpawn.position, Quaternion.identity);
                if (!isPlayerOne)
                {
                    spawnedUnit.transform.GetChild(0).rotation = Quaternion.Euler(spawnedUnit.transform.rotation.x, 180, spawnedUnit.transform.rotation.z);
                }
                pc.controlledUnits.Add(new PlayerController.MyUnits(spawnedUnit, true, unitSprite, unitHealth));
            }
        }
        foreach (var unit in unitList)
        {
            if ((int)PhotonNetwork.LocalPlayer.CustomProperties["secondUnit"] == unit.unitNumber)
            {
                GameObject unitToSpawn = unit.characterPrefab;
                Sprite unitSprite = unit.gameplaySprite;
                float unitHealth = unit.maxHP;
                GameObject spawnedUnit = PhotonNetwork.Instantiate(unitToSpawn.name, secondUnitSpawn.position, Quaternion.identity);
                if (!isPlayerOne)
                {
                    spawnedUnit.transform.GetChild(0).rotation = Quaternion.Euler(spawnedUnit.transform.rotation.x, 180, spawnedUnit.transform.rotation.z);
                }
                pc.controlledUnits.Add(new PlayerController.MyUnits(spawnedUnit, true, unitSprite, unitHealth));
            }
        }
        foreach (var unit in unitList)
        {
            if ((int)PhotonNetwork.LocalPlayer.CustomProperties["thirdUnit"] == unit.unitNumber)
            {
                GameObject unitToSpawn = unit.characterPrefab;
                Sprite unitSprite = unit.gameplaySprite;
                float unitHealth = unit.maxHP;
                GameObject spawnedUnit = PhotonNetwork.Instantiate(unitToSpawn.name, thirdUnitSpawn.position, Quaternion.identity);
                if (!isPlayerOne)
                {
                    spawnedUnit.transform.GetChild(0).rotation = Quaternion.Euler(spawnedUnit.transform.rotation.x, 180, spawnedUnit.transform.rotation.z);
                }
                pc.controlledUnits.Add(new PlayerController.MyUnits(spawnedUnit, true, unitSprite, unitHealth));
            }
        }
    }

    void UpdatePlayerList()
    {
        /*foreach (PlayerController item in bd.playerList)
        {
            Destroy(item.gameObject);
        }
        bd.playerList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }



        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {*/
        PlayerController newPlayerItem = PhotonNetwork.Instantiate(playerControllerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<PlayerController>();
        
        Debug.Log("Once");
        if (PhotonNetwork.IsMasterClient)
        {
            pcOne = newPlayerItem;
        }
        else
        {
            pcTwo = newPlayerItem;
        }

        //newPlayerItem.SetPlayerInfo(player.Value);
        /*if (PhotonNetwork.IsMasterClient)
        {
            newPlayerItem.transform.SetParent(pcSpawnOne);
            pcOne = newPlayerItem;
        } else
        {
            pcTwo = newPlayerItem;
            newPlayerItem.transform.SetParent(pcSpawnTwo);
        }
        newPlayerItem.transform.localScale = new Vector3(1, 1, 1);
        newPlayerItem.GetComponent<RectTransform>().anchoredPosition = new Vector3(1, 1, 1);
        bd.playerList.Add(newPlayerItem);*/

        /*if (player.Value == PhotonNetwork.MasterClient)
        {
            PlayerController newPlayerItem = PhotonNetwork.Instantiate(playerControllerPrefab.name, new Vector3(0,0,0), Quaternion.identity).GetComponent<PlayerController>();
            //newPlayerItem.SetPlayerInfo(player.Value);
            newPlayerItem.transform.SetParent(pcSpawnOne);
            newPlayerItem.transform.localScale = new Vector3(1, 1, 1);
            newPlayerItem.GetComponent<RectTransform>().anchoredPosition = new Vector3(1, 1, 1);
            SpawnUnits(player1Spawn, newPlayerItem);
            bd.playerList.Add(newPlayerItem);
        } else
        {
            PlayerController newPlayerItem = PhotonNetwork.Instantiate(playerControllerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<PlayerController>();
            //newPlayerItem.SetPlayerInfo(player.Value);
            newPlayerItem.transform.SetParent(pcSpawnTwo);
            newPlayerItem.transform.localScale = new Vector3(1,1,1);
            newPlayerItem.GetComponent<RectTransform>().anchoredPosition = new Vector3(1, 1, 1);

            SpawnUnits(player2Spawn, newPlayerItem);
            bd.playerList.Add(newPlayerItem);
        }*/
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
/*
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
        Debug.Log("Ada yang masuk nih");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
        Debug.Log("Ada yang keluar nih");

    }*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class UnitSpawner : MonoBehaviourPunCallbacks
{
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
    }

    public void SpawnUnits(GameObject playerSpawn, PlayerController pc)
    {
        firstUnitSpawn = playerSpawn.transform.Find("Unit 1");
        secondUnitSpawn = playerSpawn.transform.Find("Unit 2");
        thirdUnitSpawn = playerSpawn.transform.Find("Unit 3");

        foreach (var unit in unitList)
        {
            if ((int)PhotonNetwork.LocalPlayer.CustomProperties["firstUnit"] == unit.unitNumber)
            {
                GameObject unitToSpawn = unit.characterPrefab;
                GameObject spawnedUnit = PhotonNetwork.Instantiate(unitToSpawn.name, firstUnitSpawn.position, Quaternion.identity);
                pc.controlledUnits.Add(spawnedUnit);
            }
        }
        foreach (var unit in unitList)
        {
            if ((int)PhotonNetwork.LocalPlayer.CustomProperties["secondUnit"] == unit.unitNumber)
            {
                GameObject unitToSpawn = unit.characterPrefab;
                GameObject spawnedUnit = PhotonNetwork.Instantiate(unitToSpawn.name, secondUnitSpawn.position, Quaternion.identity);
                pc.controlledUnits.Add(spawnedUnit);
            }
        }
        foreach (var unit in unitList)
        {
            if ((int)PhotonNetwork.LocalPlayer.CustomProperties["thirdUnit"] == unit.unitNumber)
            {
                GameObject unitToSpawn = unit.characterPrefab;
                GameObject spawnedUnit = PhotonNetwork.Instantiate(unitToSpawn.name, thirdUnitSpawn.position, Quaternion.identity);
                pc.controlledUnits.Add(spawnedUnit);
            }
        }
    }

    void UpdatePlayerList()
    {
        foreach (PlayerController item in bd.playerList)
        {
            Destroy(item.gameObject);
        }
        bd.playerList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value == PhotonNetwork.MasterClient)
            {
                PlayerController newPlayerItem = Instantiate(playerControllerPrefab, pcSpawnOne);
                newPlayerItem.SetPlayerInfo(player.Value);
                SpawnUnits(player1Spawn, newPlayerItem);
                bd.playerList.Add(newPlayerItem);
            }
            else
            {
                PlayerController newPlayerItem = Instantiate(playerControllerPrefab, pcSpawnTwo);
                newPlayerItem.SetPlayerInfo(player.Value);
                SpawnUnits(player2Spawn, newPlayerItem);
                bd.playerList.Add(newPlayerItem);
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
        Debug.Log("Ada yang masuk nih");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
        Debug.Log("Ada yang keluar nih");

    }
}

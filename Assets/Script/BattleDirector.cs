using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using TMPro;
using UnityEngine;

public class BattleDirector : MonoBehaviour
{
    public enum PlayerTurn { PLAYER1, PLAYER2 };
    public List<PlayerController> playerList = new List<PlayerController>();
    public List<PlayerController> backupPlayerList;

    public PlayerController playerOne;
    public PlayerController playerTwo;

    PlayerTurn nowTurn;

    PhotonView view;
    public float assignedPlayerID;

    public TMP_Text turnIndicator;

    public GameObject loseScreen;
    public GameObject winScreen;
    public GameObject overlayCam;

    private const byte WIN_CONDITION = 20;

    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        
        /*if (PhotonNetwork.IsMasterClient)
        {
            Destroy(gameObject);
        }*/
        view = GetComponent<PhotonView>();

        

        if (PhotonNetwork.IsMasterClient)
        {
            yield return new WaitForSeconds(5f);
            //view.RPC("RPC_SyncBattleDirector", RpcTarget.OthersBuffered);

            view.RPC("RPC_SetPlayerTurn", RpcTarget.AllBuffered);
        }

        /*if (playerTwo.view.IsMine)
        {
            assignedPlayerID = playerTwo.view.ViewID;
            Debug.Log("You are player 2");
        }
        else if (playerOne.view.IsMine)
        {
            assignedPlayerID = playerOne.view.ViewID;
            Debug.Log("You are player 1");

        }*/

    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == WIN_CONDITION)
        {
            object[] datas = (object[])obj.CustomData;
            float enemyViewID = (float)datas[0];

            if (assignedPlayerID != enemyViewID)
            {
                winScreen.SetActive(true);
                loseScreen.SetActive(false);
            }
        }
    }

    public void PlayerLose(float playerViewID)
    {
        overlayCam.SetActive(true);
        loseScreen.SetActive(true);
        float myViewID = playerViewID;
        object[] datas = new object[] { myViewID };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(WIN_CONDITION, datas, raiseEventOptions, SendOptions.SendReliable);
    }

    [PunRPC]
    void RPC_SyncBattleDirector()
    {
        playerList.Clear();
        Debug.Log("If youre seeing this then you're player 2");
    }

    [PunRPC]
    void RPC_SetPlayerTurn()
    {
        /*backupPlayerList = new List<PlayerController>(new HashSet<PlayerController>(playerList));
        playerOne = backupPlayerList[0];
        playerTwo = backupPlayerList[1];
        */

        nowTurn = (PlayerTurn)Random.Range(0, 1);
        if (nowTurn == PlayerTurn.PLAYER1)
        {
            playerOne.thisTurn = true;
            playerOne.ResetUnit();
            playerTwo.thisTurn = false;

        }
        else
        {
            playerTwo.thisTurn = true;
            playerTwo.ResetUnit();
            playerOne.thisTurn = false;

        }
    }

    public void SwitchTurn()
    {
        if (nowTurn == PlayerTurn.PLAYER1)
        {
            nowTurn = PlayerTurn.PLAYER2;
            playerOne.thisTurn = false;
            playerTwo.thisTurn = true;
            playerTwo.ResetUnit();
        }
        else
        {
            nowTurn = PlayerTurn.PLAYER1;
            playerTwo.thisTurn = false;
            playerOne.thisTurn = true;
            playerOne.ResetUnit();
        }
        Debug.Log("ig u hav switched turns");
    }

    public void TurnIndicator(bool isMyTurn)
    {
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("SwitchTurns");
        if (isMyTurn)
        {
            turnIndicator.text = "Giliranmu!";
        }
        else
        {
            turnIndicator.text = "Giliran Musuh!";

        }
    }
}

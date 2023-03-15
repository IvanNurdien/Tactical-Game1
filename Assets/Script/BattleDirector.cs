using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BattleDirector : MonoBehaviour
{
    public enum PlayerTurn { PLAYER1, PLAYER2 };
    public List<PlayerController> playerList = new List<PlayerController>();
    public List<PlayerController> backupPlayerList;

    public PlayerController playerOne;
    public PlayerController playerTwo;

    PlayerTurn nowTurn;

    bool gameIsReady = false;

    PhotonView view;



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

    }

    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerOne);
            stream.SendNext(playerTwo);
            stream.SendNext(playerList);
            stream.SendNext(nowTurn);
        } else if (stream.IsReading)
        {
            playerOne = (PlayerController)stream.ReceiveNext();
            playerTwo = (PlayerController)stream.ReceiveNext();
            playerList = (List<PlayerController>)stream.ReceiveNext();
            nowTurn = (PlayerTurn)stream.ReceiveNext();
        }
    }*/

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            view.RPC("RPC_SwitchTurn", RpcTarget.AllBuffered);
        }*/
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
}

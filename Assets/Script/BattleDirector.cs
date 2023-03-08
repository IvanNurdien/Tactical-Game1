using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDirector : MonoBehaviour
{
    public enum PlayerTurn { PLAYER1, PLAYER2 };
    public List<PlayerController> playerList = new List<PlayerController>();

    PlayerController playerOne;
    PlayerController playerTwo;

    PlayerTurn nowTurn;

    //PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        //view = GetComponent<PhotonView>();
        foreach (PlayerController player in playerList)
        {
            if (player.thisPlayer == PhotonNetwork.MasterClient)
            {
                playerOne = player;
            }
            else
            {
                playerTwo = player;
            }
        }

        nowTurn = (PlayerTurn)Random.Range(0, 1);
        if (nowTurn == PlayerTurn.PLAYER1)
        {
            playerOne.thisTurn = true;
            playerTwo.thisTurn = false;
        } else
        {
            playerTwo.thisTurn = true;
            playerOne.thisTurn = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            view.RPC("RPC_SwitchTurn", RpcTarget.AllBuffered);
        }*/
    }

    public void SwitchTurn()
    {
        if (nowTurn == PlayerTurn.PLAYER1)
        {
            nowTurn = PlayerTurn.PLAYER2;
            playerOne.thisTurn = false;
            playerTwo.thisTurn = true;
        }
        else
        {
            nowTurn = PlayerTurn.PLAYER1;
            playerTwo.thisTurn = false;
            playerOne.thisTurn = true;
        }
        Debug.Log("ig u hav switched turns");
    }
}

using Photon.Realtime;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public bool thisTurn;
    public Player thisPlayer;
    public TMP_Text testText;
    public TMP_Text myTurn;

    public void SetPlayerInfo(Player _player)
    {
        thisPlayer = _player;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (thisTurn)
        {
            myTurn.text = "My Turn!";
        }
        else
        {
            myTurn.text = "Not My Turn";
        }
    }

    public void TestUpdate(int playerNumber)
    {
        testText.text = "I Am Player " + playerNumber;
    }
}

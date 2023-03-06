using Photon.Realtime;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public List<GameObject> controlledUnits;

    public bool thisTurn;
    public Player thisPlayer;

    public GameObject selectedUnit;
    [SerializeField] GameObject battleMenu;

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

    public void unitSelected(GameObject selectedUnit_)
    {
        if (selectedUnit_ != null)
        {
            PhotonView unitView = selectedUnit_.GetComponentInParent<PhotonView>();
            if (unitView.IsMine)
            {
                selectedUnit = selectedUnit_;
                battleMenu.SetActive(true);
            }
        }
        else
        {
            selectedUnit = null;
            battleMenu.SetActive(false);
        }
    }
}

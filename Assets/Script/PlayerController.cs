using Photon.Realtime;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // MAKING A CLASS TO LIST ALL PLAYER UNITS
    [System.Serializable]
    public class MyUnits
    {
        public GameObject Unit;
        public bool IsUnitAvail = true;
        public MyUnits(GameObject unit, bool isUnitAvail)
        {
            Unit = unit;
            IsUnitAvail = isUnitAvail;
        }
    }

    public List<MyUnits> controlledUnits;

    [SerializeField]
    BattleDirector bd;

    [SerializeField]
    PhotonView view;

    public bool thisTurn;
    public Player thisPlayer;

    public GameObject selectedUnit;
    [SerializeField] GameObject battleMenu;

    public TMP_Text testText;
    public TMP_Text myTurn;

    
    // SET PLAYER INFO
    public void SetPlayerInfo(Player _player)
    {
        thisPlayer = _player;
    }

    // Start is called before the first frame update
    void Start()
    {
        bd = GameObject.Find("Battle Director").GetComponent<BattleDirector>();
        view = GetComponent<PhotonView>();
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

    void CheckAllUnitsAvail()
    {
        bool firstUnit = controlledUnits[0].IsUnitAvail;
        bool secondUnit = controlledUnits[1].IsUnitAvail;
        bool thirdUnit = controlledUnits[2].IsUnitAvail;
        
        if (!firstUnit && !secondUnit && !thirdUnit)
        {
            Debug.Log("Nah u dont have any units left");
            view.RPC("RPC_SwitchTurn", RpcTarget.All);
        } else
        {
            Debug.Log("U good");

        }
    }

    public void unitSelected(GameObject selectedUnit_)
    {
        if (selectedUnit_ != null)
        {
            // CHECK IF THIS UNIT IS MINE OR NOT
            PhotonView unitView = selectedUnit_.GetComponentInParent<PhotonView>();
            if (unitView.IsMine)
            {
                GameObject unitName = selectedUnit_.transform.parent.gameObject;
                // CHECK IF UNITS HAS BEEN PLAYED OR NOT
                foreach (MyUnits unit in controlledUnits)
                {
                    if (unit.Unit == unitName && unit.IsUnitAvail)
                    {
                        Debug.Log("This is it chief");

                        // SET THE SELECTED UNIT VARIABLE TO THE SELECTED UNIT AND
                        // ACTIVATES THE BATTLE MENU
                        selectedUnit = selectedUnit_;
                        battleMenu.SetActive(true);
                    }
                }
            }
        }
        else
        {
            selectedUnit = null;
            battleMenu.SetActive(false);
        }
    }

    public void MoveUnit()
    {
        // GET MOVEMENT SCRIPT ON SELECTED UNIT AND CHANGE STATE TO MOVE
        MovementScript ms = selectedUnit.GetComponent<MovementScript>();
        ms.ActionSwitch(ActionType.Move, this);

        // DEACTIVATES BATTLE MENU WHEN MOVING
        battleMenu.SetActive(false);
        Debug.Log("Fired");
    }

    // SETS THE LATEST UNIT TO BE MOVED TO UNAVAILABLE
    public void EndUnitTurn(GameObject unitName)
    {
        Debug.Log("end is fired");
        foreach (MyUnits unit in controlledUnits)
        {
            if (unit.Unit == unitName)
            {
                unit.IsUnitAvail = false;
            }
        }
        CheckAllUnitsAvail();
    }



    [PunRPC]
    void RPC_SwitchTurn()
    {
        bd.SwitchTurn();
    }
}

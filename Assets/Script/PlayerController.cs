using Photon.Realtime;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ExitGames.Client.Photon;
using UnityEngine.UI;

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

    public bool isPlayerOne;

    public List<MyUnits> controlledUnits;

    [SerializeField]
    BattleDirector bd;

    [SerializeField]
    public PhotonView view;

    public bool thisTurn;
    public Player thisPlayer;

    [HideInInspector] public MouseSelect mouseSelect;
    public GameObject selectedUnit;
    [SerializeField] GameObject battleMenu;
    public GameObject confirmAtk;
    public GameObject confirmSp;

    public TMP_Text testText;
    public TMP_Text myTurn;
    [SerializeField] Image healthbarImage;

    public bool isAttacking;
    public bool isSpecial;

    const float maxHealth = 1000;
    float currentHealth = maxHealth;

    private const byte TAKE_DAMAGE = 1;
    private const byte TAKE_HEAL = 2;


    // SET PLAYER INFO
    public void SetPlayerInfo(Player _player)
    {
        thisPlayer = _player;
    }

    // Start is called before the first frame update
    void Start()
    {
        mouseSelect = GetComponent<MouseSelect>();
        bd = GameObject.Find("Battle Director").GetComponent<BattleDirector>();
        view = GetComponent<PhotonView>();

        if (view.Owner.IsMasterClient)
        {
            isPlayerOne = true;
        } else
        {
            isPlayerOne = false;
        }
        view.RPC("RPC_SetPlayerPosition", RpcTarget.AllBuffered);
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
        // TAKE DAMAGE EVENT FROM UNITS
        if (obj.Code == TAKE_DAMAGE)
        {
            // GET EVENT DATAS
            object[] datas = (object[])obj.CustomData;

            float enemyViewID = (float)datas[0];
            int damageReceived = (int)datas[1];

            // CHECKS VIEWID IS MATCHING OR NOT
            if (view.ViewID != enemyViewID)
            {
                Debug.Log("I have received " + damageReceived + "pts of damage!");
                currentHealth -= damageReceived;
                healthbarImage.fillAmount = currentHealth / maxHealth;
            }
        }
    }

    // METHOD TO RECHECK THE UNITS AVAILABILITY AFTER USING EACH
    void CheckAllUnitsAvail()
    {
        bool firstUnit = controlledUnits[0].IsUnitAvail;
        bool secondUnit = controlledUnits[1].IsUnitAvail;
        bool thirdUnit = controlledUnits[2].IsUnitAvail;
        
        // SWITCH TURNS WHEN THERES NO MORE UNITS AVAILABLE
        if (!firstUnit && !secondUnit && !thirdUnit)
        {
            Debug.Log("Nah u dont have any units left");
            view.RPC("RPC_SwitchTurn", RpcTarget.AllBuffered);
        } else
        {
            Debug.Log("U good");

        }
    }

    // RESET THE UNITS TO AVAILABLE AGAIN
    public void ResetUnit()
    {
        if (!view.IsMine) return;

        controlledUnits[0].IsUnitAvail = true;
        controlledUnits[0].Unit.GetComponentInChildren<SelectCharacter>().isPlayed = false;
        controlledUnits[1].IsUnitAvail = true;
        controlledUnits[1].Unit.GetComponentInChildren<SelectCharacter>().isPlayed = false;
        controlledUnits[2].IsUnitAvail = true;
        controlledUnits[2].Unit.GetComponentInChildren<SelectCharacter>().isPlayed = false;
    }

    public void unitSelected(GameObject selectedUnit_)
    {
        if (selectedUnit_ != null && thisTurn)
        {
            // CHECK IF THIS UNIT IS MINE OR NOT
            PhotonView unitView = selectedUnit_.GetComponentInParent<PhotonView>();
            if (unitView.IsMine)
            {
                GameObject unitName = selectedUnit_.transform.parent.gameObject;

                // CHECK IF UNITS HAS BEEN PLAYED OR NOT
                foreach (MyUnits unit in controlledUnits)
                {
                    if (unit.Unit == unitName && unit.IsUnitAvail && !isAttacking)
                    {
                        Debug.Log("This is it chief");

                        // SET THE SELECTED UNIT VARIABLE TO THE SELECTED UNIT AND
                        // ACTIVATES THE BATTLE MENU
                        CameraController.instance.followUnit = selectedUnit_.transform;
                        selectedUnit = selectedUnit_;
                        battleMenu.SetActive(true);
                    }
                }
            } else if (isAttacking)
            {
                selectedUnit.GetComponent<MovementScript>().CheckIfEnemyOnRange(selectedUnit_);
            } else if (isSpecial || unitView.IsMine)
            {
                selectedUnit.GetComponent<MovementScript>().CheckIfEnemyOnRange(selectedUnit_);
            }
        }
        else
        {
            CameraController.instance.followUnit = null;
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
        mouseSelect.isPickingUnit = false;

        Debug.Log("Fired");
    }

    public void UnitAttack()
    {
        // GET MOVEMENT SCRIPT ON SELECTED UNIT AND CHANGE STATE TO ATTACK
        MovementScript ms = selectedUnit.GetComponent<MovementScript>();
        ms.ActionSwitch(ActionType.Attack, this);
        isAttacking = true;

        // DEACTIVATES BATTLE MENU WHEN ATTACKING
        battleMenu.SetActive(false);
    }

    public void UnitSpecial()
    {
        MovementScript ms = selectedUnit.GetComponent<MovementScript>();
        ms.ActionSwitch(ActionType.Special, this);
        isSpecial = true;

        battleMenu.SetActive(false);
    }

    public void ConfirmAttack(bool accept)
    {
        MovementScript ms = selectedUnit.GetComponent<MovementScript>();
        if (accept)
        {
            ms.AttackUnit();
            isAttacking = false;
            confirmAtk.SetActive(false);

        }
        else
        {
            confirmAtk.SetActive(false);
        }
    }

    public void ConfirmSpecial(bool accept)
    {
        MovementScript ms = selectedUnit.GetComponent<MovementScript>();

        if (accept)
        {
            ms.SpecialUnit();
            isSpecial = false;
            confirmSp.SetActive(false);
        } else
        {
            confirmSp.SetActive(false);
        }
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
        mouseSelect.isPickingUnit = true;
    }

    [PunRPC]
    void RPC_SwitchTurn()
    {
        bd.SwitchTurn();
    }

    [PunRPC]
    void RPC_SetPlayerPosition()
    {
        if (isPlayerOne)
        {
            this.transform.SetParent(GameObject.Find("PC One").transform);
            bd.playerOne = this;
            
        } else
        {
            this.transform.SetParent(GameObject.Find("PC Two").transform);
            bd.playerTwo = this;
        }
        this.transform.localScale = new Vector3(1, 1, 1);
        this.GetComponent<RectTransform>().anchoredPosition = new Vector3(1, 1, 1);
    }
}

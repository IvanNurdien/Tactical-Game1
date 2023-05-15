using Photon.Realtime;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ExitGames.Client.Photon;
using UnityEngine.UI;
using Doozy.Runtime.UIManager;

public class PlayerController : MonoBehaviour
{
    // MAKING A CLASS TO LIST ALL PLAYER UNITS
    [System.Serializable]
    public class MyUnits
    {
        public GameObject Unit;
        public bool IsUnitAvail = true;
        public Sprite unitSprite;
        public Image unitSpriteUI;
        public float unitHealth;
        public MyUnits(GameObject unit, bool isUnitAvail, Sprite unitSprt, float unitHP)
        {
            Unit = unit;
            IsUnitAvail = isUnitAvail;
            unitSprite = unitSprt;
            unitHealth = unitHP;
        }
    }

    public Image firstUnitImage;
    public Image secondUnitImage;
    public Image thirdUnitImage;

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

    /*public TMP_Text testText;
    public TMP_Text myTurn;*/
    [SerializeField] Image healthbarImage;

    public bool isAttacking;
    public bool isSpecial;
    public bool isForAlly;

    float maxHealth;
    public float currentHealth;

    private const byte TAKE_DAMAGE = 1;
    private const byte TAKE_HEAL = 2;
    private const byte SYNC_UNITS = 3;
    private const byte SYNC_HEALTH = 5;


    // SET PLAYER INFO
    public void SetPlayerInfo(Player _player)
    {
        thisPlayer = _player;
    }

    // Start is called before the first frame update
    IEnumerator Start()
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

        maxHealth = controlledUnits[0].unitHealth + controlledUnits[1].unitHealth + controlledUnits[2].unitHealth;
        currentHealth = maxHealth;

        // SYNC UNIT SPRITES AND HEALTH
        string firstSprite = controlledUnits[0].unitSprite.name;
        string secondSprite = controlledUnits[1].unitSprite.name;
        string thirdSprite = controlledUnits[2].unitSprite.name;

        float myViewID = view.ViewID;
        object[] datas = new object[] { myViewID, firstSprite, secondSprite, thirdSprite, currentHealth };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        yield return new WaitForSeconds(5f);

        PhotonNetwork.RaiseEvent(SYNC_UNITS, datas, raiseEventOptions, SendOptions.SendReliable);

        //view.RPC("RPC_SyncSprites", RpcTarget.OthersBuffered, myViewID, firstSprite, secondSprite, thirdSprite);

        firstUnitImage.sprite = controlledUnits[0].unitSprite;
        secondUnitImage.sprite = controlledUnits[1].unitSprite;
        thirdUnitImage.sprite = controlledUnits[2].unitSprite;

        controlledUnits[0].unitSpriteUI = firstUnitImage;
        controlledUnits[1].unitSpriteUI = secondUnitImage;
        controlledUnits[2].unitSpriteUI = thirdUnitImage;

        
        

        if (view.IsMine)
        {
            if (thisTurn)
            {
                bd.TurnIndicator(true);
            }
            else
            {
                bd.TurnIndicator(false);

            }
        }
    }

    [PunRPC]
    public void RPC_SyncSprites(float viewID, string firstSprite, string secondSprite, string thirdSprite)
    {
        if (view.ViewID == viewID)
        {
            Debug.Log("Yep it is fired");

            firstUnitImage.sprite = Resources.Load<Sprite>("Character Sprites/" + firstSprite);
            secondUnitImage.sprite = Resources.Load<Sprite>("Character Sprites/" + secondSprite);
            thirdUnitImage.sprite = Resources.Load<Sprite>("Character Sprites/" + thirdSprite);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (thisTurn)
        {
            myTurn.text = "My Turn!";
        }
        else
        {
            myTurn.text = "Not My Turn";
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
                CheckPlayerHealth();
            }
        } else if (obj.Code == SYNC_UNITS)
        {
            object[] datas = (object[])obj.CustomData;

            float myViewID = (float)datas[0];
            string firstSprite = (string)datas[1];
            string secondSprite = (string)datas[2];
            string thirdSprite = (string)datas[3];
            float healthAmount = (float)datas[4];
            if (view.ViewID == myViewID)
            {
                Debug.Log("Yep it is fired");

                firstUnitImage.sprite = Resources.Load<Sprite>("Character Sprites/" + firstSprite);
                secondUnitImage.sprite = Resources.Load<Sprite>("Character Sprites/" + secondSprite);
                thirdUnitImage.sprite = Resources.Load<Sprite>("Character Sprites/" + thirdSprite);

                currentHealth = healthAmount;
            }
        } else if (obj.Code == TAKE_HEAL)
        {
            // HEAL PLAYER
            object[] datas = (object[])obj.CustomData;
            float targetViewID = (float)datas[0];
            int healAmount = (int)datas[1];

            if (view.ViewID == targetViewID)
            {
                currentHealth += healAmount;
                healthbarImage.fillAmount = currentHealth / maxHealth;
            }
        }
    }

    public void CheckPlayerHealth()
    {
        if (currentHealth <= 0)
        {
            bd.PlayerLose();
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
            bd.TurnIndicator(false);
        } else
        {
            Debug.Log("U good");
            isAttacking = false;
            isSpecial = false;
            isForAlly = false;
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
        bd.TurnIndicator(true);
    }

    public void unitSelected(GameObject selectedUnit_)
    {
        if (selectedUnit_ != null && thisTurn)
        {
            // CHECK IF THIS UNIT IS MINE OR NOT
            PhotonView unitView = selectedUnit_.GetComponentInParent<PhotonView>();
            if (unitView.IsMine && !isSpecial)
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
                        CameraController.instance.followUnit = selectedUnit_.transform;

                        // TURNING ON THE INDICATORS
                        if (selectedUnit == null)
                        {
                            selectedUnit = selectedUnit_;
                            selectedUnit.GetComponent<SelectCharacter>().UnitSelect(true);
                            MoveUnit(true);
                        } else
                        {
                            MoveUnit(false);
                            selectedUnit.GetComponent<SelectCharacter>().UnitSelect(false);
                            selectedUnit = selectedUnit_;
                            selectedUnit.GetComponent<SelectCharacter>().UnitSelect(true);
                            MoveUnit(true);
                        }

                        battleMenu.SetActive(true);

                        // UI INDICATOR WHICH UNIT IS ACTIVE
                        var uiColor = unit.unitSpriteUI.color;
                        uiColor.a = 1f;
                        unit.unitSpriteUI.color = uiColor;
                        
                    } else
                    {
                        var uiColor = unit.unitSpriteUI.color;
                        uiColor.a = .5f;
                        unit.unitSpriteUI.color = uiColor;
                    }
                }
                isAttacking = false;
                isSpecial = false;
            } else if (isAttacking)
            {
                selectedUnit.GetComponent<MovementScript>().CheckIfEnemyOnRange(selectedUnit_);
            } else if (isSpecial || (unitView.IsMine && isForAlly))
            {
                selectedUnit.GetComponent<MovementScript>().CheckIfEnemyOnRange(selectedUnit_);
            }
        }
        else
        {
            CameraController.instance.followUnit = null;

            MoveUnit(false);
            selectedUnit.GetComponent<SelectCharacter>().UnitSelect(false);
            selectedUnit = null;

            isAttacking = false;
            isSpecial = false;

            battleMenu.SetActive(false);
            foreach (MyUnits unit in controlledUnits)
            {
                var uiColor = unit.unitSpriteUI.color;
                uiColor.a = 1f;
                unit.unitSpriteUI.color = uiColor;
            }
        }
    }

    public void MoveUnit(bool isWalk)
    {
        MovementScript ms = selectedUnit.GetComponent<MovementScript>();
        if (isWalk)
        {
            // GET MOVEMENT SCRIPT ON SELECTED UNIT AND CHANGE STATE TO MOVE
            ms.ActionSwitch(ActionType.Move, this);

            // DEACTIVATES BATTLE MENU WHEN MOVING
            battleMenu.SetActive(true);
            //mouseSelect.isPickingUnit = false;

            Debug.Log("Fired");
        } else
        {
            ms.ActionSwitch(ActionType.None, null);
            battleMenu.SetActive(false);
        }

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
        isForAlly = ms.ActionSwitch(ActionType.Special, this);
        isSpecial = true;

        battleMenu.SetActive(false);
    }

    public void UnitEndMove()
    {
        MovementScript ms = selectedUnit.GetComponent<MovementScript>();
        ms.EndMove();
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
            ms.isPicking = true;
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
        this.GetComponent<RectTransform>().offsetMin = new Vector2(this.GetComponent<RectTransform>().offsetMin.y, 0);
        this.GetComponent<RectTransform>().offsetMax = new Vector2(this.GetComponent<RectTransform>().offsetMax.y, 0);

        
    }
}

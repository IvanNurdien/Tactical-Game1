using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ActionType
{
    None, Move, Attack, Special, Stunned
}

public enum SpecialType
{
    Damage, Heal, Stun, Buff
}

public class MovementScript : MonoBehaviourPun
{
    [System.Serializable]
    public class atkDamage
    {
        public int baseDamage;
        public int maxDamage;
        public atkDamage(int bDamage, int mDamage)
        {
            baseDamage = bDamage;
            maxDamage = mDamage;
        }
    }

    [System.Serializable]
    public class healAmount
    {
        public int baseHeal;
        public int maxHeal;
        public healAmount(int bHeal, int mHeal)
        {
            baseHeal = bHeal;
            maxHeal = mHeal;
        }
    }

    [System.Serializable]
    public class buffCounter
    {
        public bool isBuffed;
        public int buffAmount;
        public int turnCounter;
        public buffCounter(bool buffed, int amount, int turns)
        {
            isBuffed = buffed;
            buffAmount = amount;
            turnCounter = turns;
        }
    }

    private ActionType actionType;
    public SpecialType specialType;
    [SerializeField] bool isSpecialUnlimitedRange = false;
    [SerializeField] bool isForOwnUnit = false;

    public CharacterController charController;

    [SerializeField] protected float movementSpeed = 10f;
    [SerializeField] protected float rotationSpeed = 180f;
    public bool canAttack = true;
    const int specialCount = 5;
    int currentSpecialCount = specialCount;

    public int targetBuffAmount = 10;
    public buffCounter unitBuffCounter;
    bool isTargetEnemy;
    public healAmount playerHealAmount;
    public atkDamage unitAtkDamage;
    
    public GameObject target;

    [SerializeField] GameObject areaTp;
    [SerializeField] GameObject areaAtk;
    [SerializeField] GameObject areaSp;

    //public static MovementScript insMov;
    public static Animasi anm;

    public PlayerController pc;
    PlayerController enemyPC;

    SelectCharacter sc;

    private const byte GIVE_DAMAGE = 1;
    private const byte GIVE_HEAL = 2;

    private void Awake()
    {
        /*if (insMov == null)
            insMov = this;
        else if (insMov != this)
            Debug.Log("bruh this is the cause of the missing child");*/
        sc = GetComponent<SelectCharacter>();
    }
    void Update()
    {
        MoveCharacter();
        AttackMode();
        SpecialMode();
    }

    void MoveCharacter()
    {
        if (actionType != ActionType.Move) return;
        
        areaTp.SetActive(true);
        /*float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 movDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        charController.Move(movDirection * movementSpeed * Time.deltaTime);*/
        MoveUnitRelativeToCamera();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndMove();
        }
    }

    void AttackMode()
    {
        if (actionType != ActionType.Attack) return;

        areaAtk.transform.position = this.transform.position;
        areaAtk.SetActive(true);
    }

    void SpecialMode()
    {
        if (actionType != ActionType.Special) return;

        switch (specialType)
        {
            case SpecialType.Damage:
                areaSp.transform.position = this.transform.position;
                areaSp.SetActive(true);
                isTargetEnemy = true;
                break;
            case SpecialType.Stun:
                areaSp.transform.position = this.transform.position;
                areaSp.SetActive(true);
                isTargetEnemy = true;
                break;
            case SpecialType.Buff:
                if (isForOwnUnit)
                {
                    target = this.gameObject;
                    pc.confirmSp.SetActive(true);
                    pc.mouseSelect.isPickingUnit = false;

                }
                else if (!isSpecialUnlimitedRange)
                {
                    areaSp.transform.position = this.transform.position;
                    areaSp.SetActive(true);
                }
                isTargetEnemy = false;
                break;
            case SpecialType.Heal:
                int heal = Random.Range(playerHealAmount.baseHeal, playerHealAmount.maxHeal);
                float myViewID = pc.view.ViewID;
                object[] datas = new object[] { myViewID, heal };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                PhotonNetwork.RaiseEvent(GIVE_HEAL, datas, raiseEventOptions, SendOptions.SendReliable);
                EndMove();
                break;
        }
    }

    public void CheckIfEnemyOnRange(GameObject selectedUnit)
    {
        if (actionType == ActionType.Attack)
        {
            List<GameObject> enemySurroundMe = areaAtk.GetComponent<AttackArea>().enemiesInRange;
            if (enemySurroundMe.Contains(selectedUnit))
            {
                Debug.Log("yep boss he's in range");
                target = selectedUnit;
                pc.confirmAtk.SetActive(true);
            }
            else
            {
                Debug.Log("no boss");
                target = null;
                pc.confirmAtk.SetActive(false);
            }
        }
        else if (actionType == ActionType.Special)
        {
            // RANGED SPECIAL SETUP
            if (!isSpecialUnlimitedRange)
            {
                List<GameObject> unitSurroundMe = areaSp.GetComponent<AttackArea>().enemiesInRange;
                PhotonView unitView = selectedUnit.GetComponentInParent<PhotonView>();

                // IF THE SPECIAL TARGETS ENEMIES, DO THIS!
                if (isTargetEnemy)
                {
                    if (!unitView.IsMine)
                    {
                        if (unitSurroundMe.Contains(selectedUnit))
                        {
                            Debug.Log("yep boss he's in range");
                            target = selectedUnit;
                            pc.confirmSp.SetActive(true);
                        }
                        else
                        {
                            Debug.Log("no boss");
                            target = null;
                            pc.confirmSp.SetActive(false);
                        }
                    }
                }
                // ELSE, THE SPECIAL TARGETS ALLIES, DO THIS!
                else
                {
                    if (unitView.IsMine)
                    {
                        if (unitSurroundMe.Contains(selectedUnit))
                        {
                            Debug.Log("yep boss he's in range");
                            target = selectedUnit;
                            pc.confirmSp.SetActive(true);
                        }
                        else
                        {
                            Debug.Log("no boss");
                            target = null;
                            pc.confirmSp.SetActive(false);

                        }
                    }
                }
            }
            
        }
    }

    public void AttackUnit()
    {
        areaAtk.SetActive(false);
        int baseDamage = unitAtkDamage.baseDamage;
        int maxDamage = unitAtkDamage.maxDamage;
        if (unitBuffCounter.isBuffed)
        {
            baseDamage =+ unitBuffCounter.buffAmount;
            maxDamage =+ unitBuffCounter.buffAmount;
        }
        int damage = Random.Range(baseDamage, maxDamage);
        Debug.Log("I have attacked enemy's " + target.transform.parent.name + " with " + damage + "pts of damage");

        float myViewID = pc.view.ViewID;
        object[] datas = new object[] { myViewID , damage };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(GIVE_DAMAGE, datas,raiseEventOptions, SendOptions.SendReliable);
        EndMove();
    }

    public void SpecialUnit()
    {
        switch (specialType)
        {
            case SpecialType.Buff:
                areaAtk.SetActive(false);
                target.GetComponent<MovementScript>().unitBuffCounter.isBuffed = true;
                target.GetComponent<MovementScript>().unitBuffCounter.turnCounter = 4;
                target.GetComponent<MovementScript>().unitBuffCounter.buffAmount = targetBuffAmount;
                Debug.Log(target.transform.parent.name + " is buffed!");
                EndMove();
                break;
        }
    }



    void MoveUnitRelativeToCamera()
    {
        // GET PLAYER INPUT
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // GET CAMERA-NORMALIZED DIRECTION VECTORS
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;

        // CREATE DIRECTION-RELATIVE INPUT VECTORS
        Vector3 forwardRelativeVerticalInput = verticalInput * forward;
        Vector3 rightRelativeVerticalInput = horizontalInput * right;

        // CREATE CAMERA-RELATIVE INPUT
        Vector3 cameraRelativeMovement = forwardRelativeVerticalInput + rightRelativeVerticalInput;
        charController.Move(cameraRelativeMovement * movementSpeed * Time.deltaTime);
    }

    void EndMove()
    {
        if (unitBuffCounter.isBuffed)
        {
            unitBuffCounter.turnCounter--;
            if (unitBuffCounter.turnCounter == 0)
            {
                unitBuffCounter.isBuffed = false;
            }
        }

        areaTp.transform.position = this.transform.position;
        areaTp.SetActive(false);

        areaAtk.transform.position = this.transform.position;
        areaAtk.SetActive(false);

        areaSp.transform.position = this.transform.position;
        areaSp.SetActive(false);

        ActionSwitch(ActionType.None, null);
        sc.isPlayed = true;

        pc.EndUnitTurn(this.transform.parent.gameObject);
    }

    public void ActionSwitch(ActionType at, PlayerController PC)
    {
        if (pc == null)
        {
            pc = PC;
        }

        switch(at)
        {
            case ActionType.Move:
                actionType = ActionType.Move;
                break;
            case ActionType.Attack:
                actionType = ActionType.Attack;
                break;
            case ActionType.Special:
                actionType = ActionType.Special;
                break;
            case ActionType.Stunned:
                actionType = ActionType.Stunned;
                break;
            case ActionType.None:
                actionType = ActionType.None;
                break;
        }
    }
}

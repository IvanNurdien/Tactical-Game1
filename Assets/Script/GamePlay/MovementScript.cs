using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ActionType
{
    None, Move, Attack, Special, Stunned
}

public enum SpecialType
{
    Damage_T, Damage_A, Heal, Stun, Buff
}

public enum SpecialIndicator
{
    Circle, Donut, Arrow, All
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
    public class specialAmount
    {
        public int baseAmount;
        public int maxAmount;
        public specialAmount(int bAmount, int mAmount)
        {
            baseAmount = bAmount;
            maxAmount = mAmount;
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

    [Header("Actions and Its Variables")]
    public ActionType actionType;
    public SpecialType specialType;
    public SpecialIndicator attackType;
    public GameObject areaTp;
    public GameObject areaAtk;
    public GameObject areaSp;
    public GameObject attackSource;
    [SerializeField] bool isSpecialUnlimitedRange = false;
    [SerializeField] bool isForAlly = false;

    [Header("Unit Statuses")]
    public GameObject stunInd;
    public bool isStunned;
    public int stunCounter = 2;

    [Header("Action Amounts")]
    public specialAmount playerSpecialAmount;
    public atkDamage unitAtkDamage;
    public buffCounter unitBuffCounter;

    [Header("Situations")]
    public bool canAttack = true;
    public bool isPicking = true;

    [Header("Movement and Rotation Speed")]
    [SerializeField] protected float movementSpeed = 10f;
    [SerializeField] protected float rotationSpeed = 360f;
    Vector3 position;

    const int specialCount = 5;
    int currentSpecialCount = specialCount;

    public int targetBuffAmount = 10;
    
    bool isTargetEnemy;
    
    Animator anim;
    PlayerController enemyPC;

    SelectCharacter sc;

    private const byte GIVE_DAMAGE = 1;
    private const byte GIVE_BUFF = 4;
    private const byte GIVE_STUN = 8;
    private const byte GIVE_HEAL = 2;


    [Header("Ingame-Set Variables")]
    public Rigidbody rb;
    public GameObject target;
    public PlayerController pc;
    public GameObject tempSelected;
    public GameObject tempDetected;

    private void Awake()
    {
        sc = GetComponent<SelectCharacter>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        AttackMode();
        SpecialMode();
        UnitBuffed();
        UnitStunned();
    }

    private void FixedUpdate()
    {
        MoveCharacter();

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
        // TAKE DAMAGE EVENT FROM OTHER UNITS
        if (obj.Code == GIVE_DAMAGE)
        {
            // GET EVENT DATAS
            object[] datas = (object[])obj.CustomData;

            float enemyViewID = (float)datas[2];

            PhotonView unitView = this.GetComponentInParent<PhotonView>();
            // CHECK IF IM ATTACKED
            if (unitView.ViewID == enemyViewID)
            {
                Debug.Log("The one hurting is " + transform.parent.name );
            }
        } else if (obj.Code == GIVE_BUFF)
        {
            // TAKE BUFF COMMAND
            object[] datas = (object[])obj.CustomData;

            int buffAmount = (int)datas[0];
            float allyUnitView = (float)datas[1];

            PhotonView unitView = this.GetComponentInParent<PhotonView>();
            // CHECK IF ITS THE SAME UNIT
            if (unitView.ViewID == allyUnitView)
            {
                unitBuffCounter.isBuffed = true;
                unitBuffCounter.turnCounter = 4;
                unitBuffCounter.buffAmount = buffAmount;
            }
        } else if (obj.Code == GIVE_STUN)
        {
            // TAKE STUN COMMAND
            object[] datas = (object[])obj.CustomData;

            float enemyViewID = (float)datas[1];
            bool stun = (bool)datas[2];

            PhotonView unitView = GetComponentInParent<PhotonView>();
            // CHECK IF THIS UNIT IS STUNNED
            if (unitView.ViewID == enemyViewID)
            {
                isStunned = stun;
            }
        }
    }

        void MoveCharacter()
    {
        if (actionType == ActionType.None || !isPicking)
        {
            areaTp.SetActive(false);
            return;
        }
        
        areaTp.SetActive(true);
        /*float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 movDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        charController.Move(movDirection * movementSpeed * Time.deltaTime);*/
        MoveUnitRelativeToCamera();

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            EndMove();
        }*/
    }

    // TRIGGER WHEN ATTACK OPTION IS SELECTED
    void AttackMode()
    {
        if (actionType != ActionType.Attack) return;

        areaAtk.transform.position = this.transform.position;
        areaAtk.SetActive(true);

        if (isPicking)
        {
            //ARROW ROTATION
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }

            //ARROW ROTATION
            Quaternion transRot = Quaternion.LookRotation(position - transform.position);
            transRot.eulerAngles = new Vector3(0, transRot.eulerAngles.y, transRot.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transRot, transform.rotation, 0f);
        }
        

    }

    void SpecialMode()
    {
        if (actionType != ActionType.Special) return;

        switch (specialType)
        {
            case SpecialType.Damage_T:
                areaSp.transform.position = this.transform.position;
                areaSp.SetActive(true);
                isTargetEnemy = true;
                break;
            case SpecialType.Damage_A:
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
                if (isForAlly)
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
                isPicking = false;
                ActionSwitch(ActionType.None, pc);
                pc.confirmSp.SetActive(true);
                break;
        }

        if (attackType == SpecialIndicator.Arrow)
        {
            // ARROW TYPE ATTACK
            if (isPicking)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                }

                //ARROW ROTATION
                Quaternion transRot = Quaternion.LookRotation(position - transform.position);
                transRot.eulerAngles = new Vector3(0, transRot.eulerAngles.y, transRot.eulerAngles.z);

                areaSp.transform.rotation = Quaternion.Lerp(transRot, areaAtk.transform.rotation, 0f);
                transform.rotation = Quaternion.Lerp(transRot, transform.rotation, 0f);
            }
        }
    }

    public void CheckIfEnemyOnRange(GameObject selectedUnit)
    {
        if (actionType == ActionType.Attack)
        {
            List<GameObject> enemySurroundMe = areaAtk.GetComponent<AttackArea>().enemiesInRange;
            if (enemySurroundMe.Contains(selectedUnit))
            {
                Debug.Log("What you selected is " + selectedUnit.name);
                tempSelected = selectedUnit;
                RaycastHit hit;
                
                Vector3 raycastDir = selectedUnit.transform.position - attackSource.transform.position;

                if (Physics.Raycast(attackSource.transform.position, raycastDir, out hit, Mathf.Infinity) && hit.transform.gameObject == selectedUnit)
                {
                    Debug.Log("yep boss he's in range");
                    isPicking = false;
                    target = selectedUnit;
                    pc.confirmAtk.SetActive(true);
                } else
                {
                    Debug.Log("and what i detected is " + hit.transform.name);
                    tempDetected = hit.transform.gameObject;
                    isPicking = true;
                    target = null;
                    pc.confirmAtk.SetActive(false);
                }
                
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
                            isPicking = false;
                            pc.confirmSp.SetActive(true);
                        }
                        else
                        {
                            Debug.Log("no boss");
                            target = null;
                            isPicking = true;
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
                            isPicking = false;
                            pc.confirmSp.SetActive(true);
                        }
                        else
                        {
                            Debug.Log("no boss");
                            target = null;
                            isPicking = true;
                            pc.confirmSp.SetActive(false);

                        }
                    }
                }
            }
        }
    }

    public void InitiateAttack()
    {
        areaAtk.SetActive(false);

        Quaternion transRot = Quaternion.LookRotation(target.transform.position - transform.position);
        transRot.eulerAngles = new Vector3(0, transRot.eulerAngles.y, transRot.eulerAngles.z);
        transform.rotation = Quaternion.Lerp(transRot, transform.rotation, 0f);

        anim.SetTrigger("isAttack");
    }

    public void AttackUnit()
    {
        target.GetComponent<Animator>().SetTrigger("isHurt");
        
        int baseDamage = unitAtkDamage.baseDamage;
        int maxDamage = unitAtkDamage.maxDamage;

        // ATTACK AMOUNT IF BUFFED
        if (unitBuffCounter.isBuffed)
        {
            baseDamage += unitBuffCounter.buffAmount;
            maxDamage += unitBuffCounter.buffAmount;
        }
        // END BUFF DAMAGE

        int damage = Random.Range(baseDamage, maxDamage);

        float myViewID = pc.view.ViewID;
        float enemyUnitViewID = target.transform.parent.GetComponent<PhotonView>().ViewID;
        object[] datas = new object[] { myViewID, damage, enemyUnitViewID };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(GIVE_DAMAGE, datas, raiseEventOptions, SendOptions.SendReliable);
        EndMove();
        Debug.Log("I have attacked enemy's " + target.transform.parent.name + " with " + damage + "pts of damage");

    }

    public void InitiateSpecial()
    {
        areaSp.SetActive(false);

        anim.SetTrigger("isSpecial");
    }

    public void SpecialUnit()
    {
        int baseDamage;
        int maxDamage;
        float myViewID;
        float enemyUnitViewID;
        object[] datas;
        RaiseEventOptions raiseEventOptions;

        switch (specialType)
        {
            case SpecialType.Buff:
                areaSp.SetActive(false);
                baseDamage = targetBuffAmount;
                enemyUnitViewID = target.GetComponentInParent<PhotonView>().ViewID;
                datas = new object[] { baseDamage, enemyUnitViewID };
                raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                PhotonNetwork.RaiseEvent(GIVE_BUFF, datas, raiseEventOptions, SendOptions.SendReliable);


                Debug.Log(target.transform.parent.name + " is buffed!");
                EndMove();
                break;

            case SpecialType.Damage_A:
                // LIST ALL THE UNITS SURROUNDING
                List<GameObject> unitSurroundMe = areaSp.GetComponent<AttackArea>().enemiesInRange;
                
                areaSp.SetActive(false);
                baseDamage = playerSpecialAmount.baseAmount;
                maxDamage = playerSpecialAmount.maxAmount;

                // ATTACK AMOUNT IF BUFFED
                if (unitBuffCounter.isBuffed)
                {
                    baseDamage += unitBuffCounter.buffAmount;
                    maxDamage += unitBuffCounter.buffAmount;
                }
                // END BUFF DAMAGE

                int damage = Random.Range(baseDamage, maxDamage);
                // DAMAGES ALL THE ENEMY SURROUNDING ME
                foreach (GameObject unit in unitSurroundMe)
                {
                    PhotonView unitView = unit.GetComponentInParent<PhotonView>();
                    if (!unitView.IsMine)
                    {
                        unit.GetComponent<Animator>().SetTrigger("isHurt");

                        myViewID = pc.view.ViewID;
                        enemyUnitViewID = unit.transform.parent.GetComponent<PhotonView>().ViewID;
                        datas = new object[] { myViewID, damage, enemyUnitViewID };
                        raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                        PhotonNetwork.RaiseEvent(GIVE_DAMAGE, datas, raiseEventOptions, SendOptions.SendReliable);
                        Debug.Log("I do Special!");
                    }
                }
                EndMove();
                break;

            case SpecialType.Damage_T:
                target.GetComponent<Animator>().SetTrigger("isHurt");

                areaSp.SetActive(false);
                baseDamage = playerSpecialAmount.baseAmount;
                maxDamage = playerSpecialAmount.maxAmount;

                // ATTACK AMOUNT IF BUFFED
                if (unitBuffCounter.isBuffed)
                {
                    baseDamage += unitBuffCounter.buffAmount;
                    maxDamage += unitBuffCounter.buffAmount;
                }
                // END BUFF DAMAGE


                damage = Random.Range(baseDamage, maxDamage);
                myViewID = pc.view.ViewID;
                enemyUnitViewID = target.GetComponentInParent<PhotonView>().ViewID;
                datas = new object[] { myViewID, damage, enemyUnitViewID };

                raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                PhotonNetwork.RaiseEvent(GIVE_DAMAGE, datas, raiseEventOptions, SendOptions.SendReliable);
                EndMove();
                break;
            case SpecialType.Heal:
                int heal = Random.Range(playerSpecialAmount.baseAmount, playerSpecialAmount.maxAmount);
                myViewID = pc.view.ViewID;
                datas = new object[] { myViewID, heal };
                raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                PhotonNetwork.RaiseEvent(GIVE_HEAL, datas, raiseEventOptions, SendOptions.SendReliable);
                EndMove();
                break;
            case SpecialType.Stun:
                myViewID = pc.view.ViewID;
                enemyUnitViewID = target.GetComponentInParent<PhotonView>().ViewID;
                bool stun = true;
                datas = new object[] { myViewID, enemyUnitViewID, stun };

                raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                PhotonNetwork.RaiseEvent(GIVE_STUN, datas, raiseEventOptions, SendOptions.SendReliable);
                EndMove();
                break;
             
        }
    }

    void UnitStunned()
    {
        if (isStunned)
        {
            stunInd.SetActive(true);
            sc.isPlayed = true;
        } else
        {
            stunInd.SetActive(false);
            stunCounter = 2;
        }
    }

    void UnitBuffed()
    {
        if (unitBuffCounter.isBuffed)
        {
            // Activates something
        } else
        {
            // Deactivates something
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
        forward = forward.normalized;
        right = right.normalized;

        // CREATE DIRECTION-RELATIVE INPUT VECTORS
        Vector3 forwardRelativeVerticalInput = verticalInput * forward;
        Vector3 rightRelativeVerticalInput = horizontalInput * right;

        // CREATE CAMERA-RELATIVE INPUT
        Vector3 cameraRelativeMovement = forwardRelativeVerticalInput + rightRelativeVerticalInput;

        transform.Translate(cameraRelativeMovement * movementSpeed * Time.fixedDeltaTime, Space.World);
        if (cameraRelativeMovement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(cameraRelativeMovement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
            anim.SetFloat("isWalk", 1);
        }
        else
        {
            anim.SetFloat("isWalk", 0);

        }

    }

    public void EndMove()
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
        sc.UnitSelect(false);

        pc.EndUnitTurn(this.transform.parent.gameObject);
    }

    public bool ActionSwitch(ActionType at, PlayerController PC)
    {
        if (pc == null)
        {
            pc = PC;
        }

        switch(at)
        {
            case ActionType.Move:
                isPicking = true;
                actionType = ActionType.Move;
                break;
            case ActionType.Attack:
                actionType = ActionType.Attack;
                isPicking = true;
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

        return isForAlly;
    }
}

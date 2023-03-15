using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
//using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ActionType
{
    None, Move, Attack, Special
}
public class MovementScript : MonoBehaviourPun
{
    [System.Serializable]
    public class atkDamage
    {
        public float baseDamage;
        public float maxDamage;
        public atkDamage(float bDamage, float mDamage)
        {
            baseDamage = bDamage;
            maxDamage = mDamage;
        }
    }

    private ActionType actionType;
    public CharacterController charController;

    [SerializeField] protected float movementSpeed = 10f;
    [SerializeField] protected float rotationSpeed = 180f;
    public bool canAttack = true;
    public atkDamage unitAtkDamage;
    public GameObject target;

    [SerializeField] GameObject areaTp;
    [SerializeField] GameObject areaAtk;

    //public static MovementScript insMov;
    public static Animasi anm;

    PlayerController pc;
    PlayerController enemyPC;

    SelectCharacter sc;

    private const byte GIVE_DAMAGE = 0;

    private void Awake()
    {
        /*if (insMov == null)
            insMov = this;
        else if (insMov != this)
            Debug.Log("bruh this is the cause of the missing child");*/
        sc = GetComponent<SelectCharacter>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    void Update()
    {
        MoveCharacter();
        AttackMode();
    }

    

    public void BtnAttack()
    {
        if (canAttack)
        {
            areaTp.SetActive(false);
            canAttack = false;
        }
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

    public void CheckIfEnemyOnRange(GameObject enemyUnit)
    {
        List<GameObject> enemySurroundMe = areaAtk.GetComponent<AttackArea>().enemiesInRange;
        foreach (GameObject enemy in enemySurroundMe)
        {
            if (enemy == enemyUnit)
            {
                Debug.Log("yep boss he's in range");
                target = enemyUnit;
            }
        }
    }

    public void AttackUnit()
    {
        areaAtk.SetActive(false);
        float damage = Random.Range(unitAtkDamage.baseDamage, unitAtkDamage.maxDamage);
        Debug.Log("I have attacked enemy's " + target.transform.parent.name + " with " + damage + "pts of damage");

        float enemyViewID = pc.view.ViewID;
        object[] datas = new object[] { enemyViewID , damage };
        PhotonNetwork.RaiseEvent(GIVE_DAMAGE, datas,RaiseEventOptions.Default, SendOptions.SendReliable);
        EndMove();
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
        areaTp.transform.position = this.transform.position;
        areaTp.SetActive(false);

        areaAtk.transform.position = this.transform.position;
        areaAtk.SetActive(false);

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
            case ActionType.None:
                actionType = ActionType.None;
                break;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType
{
    None, Move, Attack, Special
}
public class MovementScript : MonoBehaviour
{
    private ActionType actionType;
    public CharacterController charController;
    [SerializeField] protected float movementSpeed = 10f;
    [SerializeField] protected float rotationSpeed = 180f;
    [SerializeField] GameObject areaTp;
    public static MovementScript insMov;
    public bool canAttack = true;
    public static Animasi anm;

    PlayerController pc;

    SelectCharacter sc;

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
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 movDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        charController.Move(movDirection * movementSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndMove();
        }
    }

    void EndMove()
    {
        areaTp.transform.position = this.transform.position;
        areaTp.SetActive(false);
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

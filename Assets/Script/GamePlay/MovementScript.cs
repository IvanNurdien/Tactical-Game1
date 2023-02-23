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

    private void Awake()
    {
        if (insMov == null)
            insMov = this;
        else if (insMov != this)
            Destroy(gameObject);
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
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 movDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        charController.Move(movDirection * movementSpeed * Time.deltaTime);

        

    }
    public void ActionSwitch(ActionType at)
    {
        switch(at)
        {
            case ActionType.Move:
                actionType = ActionType.Move;
                break;
            case ActionType.Attack:
                break;
            case ActionType.Special:
                break;
        }
    }
}

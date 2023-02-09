using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public CharacterController charController;
    [SerializeField] protected float movementSpeed = 10f;
    [SerializeField] protected float rotationSpeed = 180f;
    [SerializeField] MovementScript ins;
    private void Awake()
    {
        ins = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
    }
    void MoveCharacter()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 movDirection = new Vector3(horizontalInput, 0, verticalInput);
        charController.Move(movDirection * movementSpeed * Time.deltaTime);
    }

}

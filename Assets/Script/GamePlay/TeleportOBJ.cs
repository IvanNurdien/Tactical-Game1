using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportOBJ : MonoBehaviour
{
    public Transform teleportTarget;
    public GameObject theArea;
    public float timeLeft;
    public float timeCD;

    private void Awake()
    {
        timeLeft = timeCD;
    }

    public void MoveArea()
    {
        theArea.transform.position = teleportTarget.transform.position;
    }

    /*private void OnTriggerEnter(Collider other)
    {
        //TheArea.transform.position = teleportTarget.transform.position;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CapsuleCollider>())
        {
            TheArea.transform.position = teleportTarget.transform.position;
        }
    }*/
}

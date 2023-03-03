using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{
    private Renderer renderer;

    public string playerTag;

    PhotonView view;

    
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponentInParent<PhotonView>();
        renderer = GetComponent<Renderer>();

        playerTag = this.gameObject.tag;

        Debug.Log(playerTag);
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }


    // Detect Mouse by changing color
    private void OnMouseEnter()
    {
        if (view.IsMine)
        {
            if (playerTag == "Player 1")
            {
                renderer.material.color = Color.blue;
            }
            else
            {
                renderer.material.color = Color.red;
            }
        }
    }

    private void OnMouseExit()
    {
        renderer.material.color = Color.white;
    }
}

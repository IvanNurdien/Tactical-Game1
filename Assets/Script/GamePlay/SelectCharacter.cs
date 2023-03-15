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

    public bool isPlayed = false;

    
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponentInParent<PhotonView>();
        renderer = GetComponent<Renderer>();

        playerTag = this.gameObject.tag;

        Debug.Log(playerTag);
    }


    // Detect Mouse by changing color
    private void OnMouseEnter()
    {
        if (view.IsMine)
        {
            if (isPlayed)
            {
                renderer.material.color = Color.yellow;
            } else
            {
                renderer.material.color = Color.blue;
            }
            
        } else
        {
            renderer.material.color = Color.red;
        }
    }

    private void OnMouseExit()
    {
        renderer.material.color = Color.white;
    }
}

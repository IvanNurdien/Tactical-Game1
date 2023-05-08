using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{
    public string playerTag;

    PhotonView view;
    MovementScript script;

    public bool isPlayed = false;
    bool isSelected;

    Material indicatorMaterial;

    [SerializeField] Color enemyColor;
    [SerializeField] Color playedColor;
    [SerializeField] Color hoverColor;

    public GameObject unitInd;

    
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponentInParent<PhotonView>();

        indicatorMaterial = unitInd.GetComponent<SpriteRenderer>().material;

        script = GetComponent<MovementScript>();


        playerTag = this.gameObject.tag;

        Debug.Log(playerTag);
    }

    private void Update()
    {
        if (isSelected)
        {
            unitInd.SetActive(true);
        }
    }

    public void UnitSelect(bool unitSelected)
    {
        if (unitSelected)
        {
            indicatorMaterial.SetFloat("_Emission", 1);
            isSelected = true;
        } else
        {
            indicatorMaterial.SetFloat("_Emission", 0);
            isSelected = false;
            unitInd.SetActive(false);
            script.areaAtk.SetActive(false);
            script.areaSp.SetActive(false);
            script.areaTp.SetActive(false);
        }
        
    }


    // Detect Mouse by changing color
    private void OnMouseEnter()
    {
        if (view.IsMine)
        {
            if (isPlayed)
            {
                Color tada = new Color(251, 255, 142);

                unitInd.SetActive(true);
                indicatorMaterial.SetColor("_MainColor", playedColor);
                Debug.Log("Iam played and the mouse is on me");

            }
            else
            {
                Color tada = new Color(142, 249, 253);

                unitInd.SetActive(true);
                indicatorMaterial.SetColor("_MainColor", hoverColor);
                Debug.Log("Iam not played and the mouse is on me");

            }

        } else
        {
            unitInd.SetActive(true);
            Debug.Log("Iam the enemy and the mouse is on me");

        }
    }

    private void OnMouseExit()
    {
        unitInd.SetActive(false);
        Debug.Log("Mouse is not on me");
    }
}

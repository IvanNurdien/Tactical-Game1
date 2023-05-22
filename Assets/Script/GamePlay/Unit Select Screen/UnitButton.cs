using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Unit units;
    TMP_Text buttonText;
    private PhotonView PV;

    string headerContent;
    string messageContent;

    string spType;
    string moveRange;
    string attackRange;


    void Start()
    {
        PV = GetComponent<PhotonView>();
        buttonText = transform.Find("Unit Name").GetComponent<TextMeshProUGUI>();

        buttonText.text = units.UnitName;

        switch (units.spType)
        {
            case Unit.SpecialType.Damage_T:
                spType = "Serang 1 Unit Musuh";
                break;
            case Unit.SpecialType.Damage_A:
                spType = "Serang Area";
                break;
            case Unit.SpecialType.Buff:
                spType = "Menguatkan Unit Teman";
                break;
            case Unit.SpecialType.Heal:
                spType = "Menyembuhkan Pemain";
                break;
            case Unit.SpecialType.Stun:
                spType = "Memusingkan Unit Lawan";
                break;
        }

        headerContent = units.UnitName;

        messageContent = "Darah: " + units.maxHP +
            "\nDaya Serang: " + units.damage +
            "\nTipe Spesial: " + spType +
            "\nUnit Jarak " + units.moveRange + " dengan Serangan yang " + units.attackRange;
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Show(headerContent, messageContent);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
    }

    // Start is called before the first frame update
    

    /*public void SelectedByOtherPlayer(bool isSelected)
    {
        PV.RPC("RPC_CharSelectedByOtherPlayer", RpcTarget.OthersBuffered, isSelected);
    }

    [PunRPC]
    void RPC_CharSelectedByOtherPlayer(bool isSelected)
    {
        if (isSelected)
        {
            transform.Find("Player2Chosen").gameObject.SetActive(true);
        } else
        {
            transform.Find("Player2Chosen").gameObject.SetActive(false);
        }
    }*/
}

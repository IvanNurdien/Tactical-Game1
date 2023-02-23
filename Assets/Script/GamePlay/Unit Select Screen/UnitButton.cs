using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitButton : MonoBehaviour
{
    public Unit units;
    TMP_Text buttonText;
    private PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        buttonText = transform.Find("Unit Name").GetComponent<TextMeshProUGUI>();

        buttonText.text = units.UnitName;
    }

    public void SelectedByOtherPlayer()
    {
        PV.RPC("RPC_CharSelectedByOtherPlayer", RpcTarget.OthersBuffered);
    }

    [PunRPC]
    void RPC_CharSelectedByOtherPlayer()
    {
       transform.Find("Player2Chosen").gameObject.SetActive(true);
    }
}

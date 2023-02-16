using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitButton : MonoBehaviour
{
    public Unit units;
    TMP_Text buttonText;

    // Start is called before the first frame update
    void Start()
    {
        buttonText = transform.Find("Unit Name").GetComponent<TextMeshProUGUI>();

        buttonText.text = units.UnitName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

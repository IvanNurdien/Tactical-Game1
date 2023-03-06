using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionUI : MonoBehaviour
{
    [SerializeField] private Button MoveBtn;
    [SerializeField] private Button AttacBtn;
    [SerializeField] private Button SpecialBtn;
    [SerializeField] private GameObject characterUnit;

    private void Awake()
    {
        MoveBtn.onClick.AddListener(() =>
        {
            characterUnit.GetComponent<MovementScript>().ActionSwitch(ActionType.Move);

        });
    }
}

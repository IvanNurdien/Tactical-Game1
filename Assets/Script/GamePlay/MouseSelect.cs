using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PLAYERSELECT {PLAYER_1, PLAYER_2}
public class MouseSelect : MonoBehaviour
{
    private BattleSystem battleSystem;

    public Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        battleSystem = GameObject.Find("/Battlehandller").GetComponent<BattleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.gameObject.GetComponent<SelectCharacter>() != null)
                {
                    battleSystem.selectedChar = hitInfo.collider.gameObject;
                    battleSystem.CharacterSelected();
                } else
                {
                    battleSystem.selectedChar = null;
                }
            }
            else
            {
                battleSystem.selectedChar = null;
            }
        }
    }
}

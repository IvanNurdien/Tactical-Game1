using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PLAYERSELECT {PLAYER_1, PLAYER_2}
public class MouseSelect : MonoBehaviour
{
    public bool isPickingUnit = true;

    private PlayerController pc;

    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<PlayerController>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) )
        {
            if (!PointerOverUI())
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    if (hitInfo.collider.gameObject.GetComponent<SelectCharacter>() != null && isPickingUnit)
                    {
                        pc.unitSelected(hitInfo.collider.gameObject);
                    }
                    else if (!pc.isAttacking)
                    {
                        CameraController.instance.followUnit = null;

                        pc.unitSelected(null);
                    }
                }
                else if (!pc.isAttacking)
                {
                    CameraController.instance.followUnit = null;

                    pc.unitSelected(null);
                }
            }
        }
    }

    public bool PointerOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }
}

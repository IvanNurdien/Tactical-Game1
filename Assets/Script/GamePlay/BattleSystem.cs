using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataCharacter;

public enum BattleState { START,PLAYERTUURN,ENEMYTURN,WON,LOST}
public class BattleSystem : MonoBehaviour
{
    [SerializeField] private Unit [] characterData;
    /*public GameObject playerPrefab;
    public GameObject enemyprefab;
    public Transform playerBattlestation;
    public Transform enemyBattlestation;
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;*/
    Unit playerUnit;
    Unit enemyUnit;
    public BattleState state;
    public GameObject turnIndicator;

    public GameObject selectedChar;

    [SerializeField] TeleportOBJ areaTp;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetUpBattle());
    }

    // Update is called once per frame
    void Update()
    {
        if (state == BattleState.PLAYERTUURN)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                OnAttackButton();
            }
        }
    }
    IEnumerator SetUpBattle()
    {
        /*GameObject playerGO = Instantiate(playerPrefab, playerBattlestation);
        playerUnit = playerGO.GetComponent<Unit>();
        GameObject enemyGO = Instantiate(enemyprefab, playerBattlestation);
        enemyUnit = playerGO.GetComponent<Unit>();*/

        /*playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);*/

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTUURN;
        PlayerTurn();
    }
    IEnumerator PlayerAttack()
    {
        yield return new WaitForSeconds(2f);

        areaTp.theArea.SetActive(false);
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
        
    }

    IEnumerator EnemyTurn()
    {
        Debug.Log("Giliran musuh woi");
        turnIndicator.SetActive(true);

        yield return new WaitForSeconds(2f);

        Debug.Log("Musuh ngapain kek");

        yield return new WaitForSeconds(1f);

        state = BattleState.PLAYERTUURN;
        PlayerTurn();
    }
    void PlayerTurn()
    {
        areaTp.theArea.SetActive(true);
        areaTp.MoveArea();
        turnIndicator.SetActive(false);
        Debug.Log("Giliran Player woi"); 
    }

    public void OnAttackButton ()
    {
        if (state != BattleState.PLAYERTUURN)
            
            return;

        StartCoroutine(PlayerAttack());
        Debug.Log("playerAttack");
    }
    public void OnMoveButton()
    {
        selectedChar.GetComponent<MovementScript>().enabled = true;
    }

    public void CharacterSelected()
    {
        Debug.Log("Character " + selectedChar.gameObject.name + " is selected");
    }
}

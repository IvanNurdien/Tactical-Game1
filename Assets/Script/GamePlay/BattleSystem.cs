using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject en;
    Unit playerUnit;
    Unit enemyUnit;
    public BattleState state;
    public GameObject turnIndicator;
    public static BattleSystem insBatSy;
    public GameObject selectedChar;
    public Animasi anim;
    public float turnTimeLimit = 10f; // batas waktu satu giliran
    private float turnTimer; // timer untuk menghitung waktu satu giliran
    public GameObject panelPlayer;
    public GameObject panelEnemy;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetUpBattle());
        turnTimer = turnTimeLimit;
        panelPlayer.SetActive(true);
        panelEnemy.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (state == BattleState.PLAYERTUURN)
        {
            turnTimer = turnTimeLimit;
            if (turnTimer <= 0) // jika waktunya abis pindah ke lawan.
            {
                state = BattleState.ENEMYTURN;
                EnemyTurn();
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
        Debug.Log("musuh genten cok");
        panelPlayer.SetActive(false);
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
        
    }

    IEnumerator EnemyTurn()
    {
        Debug.Log("Giliran musuh woi");
        turnIndicator.SetActive(true);
        panelEnemy.SetActive(true);
        yield return new WaitForSeconds(3f);

        Debug.Log("Musuh ngapain kek");

        yield return new WaitForSeconds(2f);
        panelPlayer.SetActive(true);
        panelEnemy.SetActive(false);
        state = BattleState.PLAYERTUURN;
        PlayerTurn();
    }
    void PlayerTurn()
    {
        Debug.Log("Giliran Player woi");

        anim.AttackAnim();

        turnIndicator.SetActive(false);
    }

    public void OnAttackButton ()
    {
        if (state != BattleState.PLAYERTUURN)
            
            return;
        en.SetActive(false);
        
        StartCoroutine(PlayerAttack());
        Debug.Log("playerAttack");
        EnemyTurn();    
        
    }
    

    public void CharacterSelected()
    {
        Debug.Log("Character " + selectedChar.gameObject.name + " is selected");
    }
    public void EndTurn()
    {
        Debug.Log("End Turn");
        StartCoroutine(PlayerAttack());
    }
}

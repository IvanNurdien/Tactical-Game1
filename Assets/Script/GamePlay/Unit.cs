using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*namespace DataCharacter
{*/
// scriptable sctipt character
[CreateAssetMenu(fileName = "CharacterData",
    menuName = "Character(s)/AddNewCharacter", order = 1)]
public class Unit : ScriptableObject
{
    [SerializeField]
    public enum SpecialType
    {
        Damage_T, Damage_A, Heal, Stun, Buff
    }
    [SerializeField]
    public enum MoveRange
    {
        Dekat, Medium, Jauh
    }
    [SerializeField]
    public enum AttackRange
    {
        Dekat, Medium, Luas
    }

    public GameObject characterPrefab;
    [TextArea(1, 3)]
    public string Description;
    public string UnitName;
    public int unitNumber;
    public int damage;
    public SpecialType spType;
    public MoveRange moveRange;
    public AttackRange attackRange;
    public int maxHP;
    public int currentHp;
    public Sprite characterSelectSprite;
    public Sprite gameplaySprite;


    public bool TakeDamage(int dmg)
    {
        currentHp -= dmg;
        if (currentHp <= 0)
            return true;
        else
            return false;
    }
}
//}

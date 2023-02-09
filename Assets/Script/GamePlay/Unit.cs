using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataCharacter
{
    // scriptable sctipt character
    [CreateAssetMenu(fileName = "CharacterData",
        menuName = "Character(s)/AddNewCharacter", order = 1)]
    public class Unit : ScriptableObject
    {
        GameObject characterPrefab;
        [TextArea(20, 40)]
        public string Description;
        public string UnitName;
        public int damage;
        public int maxHP;
        public int currentHp;


        public bool TakeDamage(int dmg)
        {
            currentHp -= dmg;
            if (currentHp <= 0)
                return true;
            else
                return false;
        }
    }
}

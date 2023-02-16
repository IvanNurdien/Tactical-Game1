using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*namespace DataCharacter
{*/
    // scriptable sctipt character
    [CreateAssetMenu(fileName = "CharacterData",
        menuName = "Character(s)/AddNewCharacter", order = 1)]
    public class Unit : ScriptableObject
    {
        public GameObject characterPrefab;
        [TextArea(1, 3)]
        public string Description;
        public string UnitName;
        public int unitNumber;
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
//}

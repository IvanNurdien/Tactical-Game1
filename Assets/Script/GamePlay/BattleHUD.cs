using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Slider hpSlider;
    public Text nameText;
    public void SetHUD(Unit unit)
    {
        nameText.text = unit.UnitName;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHp;
    }
    public void SetHp(int hp)
    {
        hpSlider.value = hp;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSummoner : MonoBehaviour
{
    public GameObject theSlime;
    Animator slimeAnim;
    // Start is called before the first frame update
    void Start()
    {
        slimeAnim = theSlime.GetComponent<Animator>();
    }

    public void SlimeAttack()
    {
        slimeAnim.SetTrigger("isAttack");
    }

    public void SlimeSpecial()
    {
        slimeAnim.SetTrigger("isSpecial");
    }

    public void AttackUnit()
    {
        GetComponentInParent<MovementScript>().AttackUnit();
    }

    public void SpecialUnit()
    {
        GetComponentInParent<MovementScript>().SpecialUnit();
    }
}

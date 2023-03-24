using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Anim : MonoBehaviour
{
    public Animator anim;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            anim.SetBool("jalan", true);
        }
        else
        {
            anim.SetBool("jalan", false);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("lari", true);
            //audioManager.singleton.playAudio(1);
        }
        else
        {
            anim.SetBool("lari", false);
        }
    }
    IEnumerator delayBetweenWalk()
    {
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(delayBetweenWalk());
    }
}

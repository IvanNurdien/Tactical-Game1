using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Animasi : MonoBehaviour
{
    public Animator animasi;
    Rigidbody rb;
    public static Animasi insAnim;
    // Start is called before the first frame update
    void Start()
    {
        animasi = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            animasi.SetTrigger("isWalk");
        }


    }
    IEnumerator delayBetweenWalk()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(delayBetweenWalk());
    }
     public void AttackAnim()
    {
        animasi.SetTrigger("isAttack");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    
    Animator animator;
    SpriteRenderer spriteRenderer;
       
    PlayerStatus playerstatus;
    public ControllerPlayer controller;
    
    private bool isDead = false;
    
    public void Awake()
    {
        playerstatus = GetComponent<PlayerStatus>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        controller = GetComponent<ControllerPlayer>();
        //rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartCoroutine(TestGetDamage()); //테스트용
        }

        if (isDead)
        {
            animator.SetBool("IsDead", true);
        }
    }

    public void GetDamage(float damage)
    {
        playerstatus.stats[StatType.HP] -= damage;
        
        if (playerstatus.stats[StatType.HP] <= 0) 
        {
            isDead = true;
        }
    }

    IEnumerator TestGetDamage() //피격 판정 테스트용 
    {
        GetDamage(10);
        animator.SetTrigger("HitTrigger");
        yield return new WaitForSeconds(0.5f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    Animator animator;
       
    PlayerStatus playerstatus;
    //Rigidbody2D rigidbody;

    private bool isDead = false;

    public void Awake()
    {
        playerstatus = GetComponent<PlayerStatus>();
        animator = GetComponent<Animator>();
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
        animator.SetBool("IsDamaged", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("IsDamaged", false);
       
    }

}

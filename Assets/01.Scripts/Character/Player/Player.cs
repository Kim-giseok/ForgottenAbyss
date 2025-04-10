using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    Animator animator;
       
    PlayerStatus playerstatus;

    private bool isDead = false;

    public void Awake()
    {
        playerstatus = GetComponent<PlayerStatus>();
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        StartCoroutine(TestGetDamage());

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

    IEnumerator TestGetDamage()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            GetDamage(10);
            animator.SetBool("IsDamaged", true);
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("IsDamaged", false);
        }
    }

}

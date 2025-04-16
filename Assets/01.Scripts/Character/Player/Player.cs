using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    
    public Animator animator;
    SpriteRenderer spriteRenderer;
       
    PlayerStatus playerstatus;
    public ControllerPlayer controller;
    
    private bool isDead;
    
    public void Awake()
    {
        playerstatus = GetComponent<PlayerStatus>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        controller = GetComponent<ControllerPlayer>();
        //rigidbody = GetComponent<Rigidbody2D>();

        isDead = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartCoroutine(TestGetDamage()); //테스트용
        }

        if (isDead)
        {
            StartCoroutine(TimeSet());
        }
    }

    public void GetDamage(float damage)
    {
        playerstatus.stats[StatType.HP] -= damage;

        if (playerstatus.stats[StatType.HP] <= 0)
        {
            animator.SetTrigger("DeadTrigger");

            isDead = true;
        }

        animator.SetTrigger("HitTrigger");

        SkillController.Instance.SetGettingHit(true);
        SkillController.Instance.ResetAttack();

        StartCoroutine(ClearGettingHitAfterDelay(0.4f));
    }

    IEnumerator TestGetDamage() //피격 판정 테스트용 
    {
        GetDamage(50);
        
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator ClearGettingHitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SkillController.Instance.SetGettingHit(false);
    }

    IEnumerator TimeSet()
    {
        yield return new WaitForSeconds(1.5f);

        Time.timeScale = 0f;
    }
}

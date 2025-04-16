using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Player : MonoBehaviour, IDamagable
{
    
    public Animator animator;
    SpriteRenderer spriteRenderer;
       
    PlayerStatus playerstatus;
    ControllerPlayer controller;
    SkillController skillController;
    
    public bool isDead;
    
    public void Awake()
    {
        playerstatus = GetComponent<PlayerStatus>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        controller = GetComponent<ControllerPlayer>();
        //rigidbody = GetComponent<Rigidbody2D>();

        isDead = false;
    }

    private void Start()
    {
        StartCoroutine(SetupSkillController());
    }

    IEnumerator SetupSkillController()
    {
        while (SkillController.Instance == null)
            yield return null;

        skillController = SkillController.Instance;
        skillController.comboAttack = GetComponent<ComboAttack>();
        skillController.rangedAttack = GetComponent<RangedAttack>();

        Debug.Log("[Player] SkillController 세팅 완료");
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
            skillController.SetDead(true);
            controller.isAlive = false;
            isDead = true;
        }

        animator.SetTrigger("HitTrigger");

        skillController.SetGettingHit(true);
        skillController.ResetAttack();

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
        skillController.SetGettingHit(false);
    }

    IEnumerator TimeSet()
    {
        yield return new WaitForSeconds(1.5f);

        Time.timeScale = 0f;
    }
}

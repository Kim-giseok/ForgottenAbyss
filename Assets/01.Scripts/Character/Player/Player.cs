using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamagable
{
    
    public Animator animator;
    SpriteRenderer spriteRenderer;
       
    PlayerStatus playerstatus;
    public ControllerPlayer controller;
    SkillController skillController;
    
    public bool isDead = false;

    public void Awake()
    {
        playerstatus = GetComponent<PlayerStatus>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        controller = GetComponent<ControllerPlayer>();
        //rigidbody = GetComponent<Rigidbody2D>();
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
    }

    public void GetDamage(float damage)
    {
        playerstatus.stats[StatType.HP] -= damage;

        if (playerstatus.stats[StatType.HP] <= 0)
        {
            Die();
        }
        else
            Hit();
    }

    IEnumerator TestGetDamage() //피격 판정 테스트용 
    {
        GetDamage(50);
        
        yield return new WaitForSeconds(0.5f);
    }

    void Die()
    {
        Debug.Log("die 실행");
        if (isDead) return;

        isDead = true;    
        controller.isAlive = false;
        controller.rigid.velocity = Vector2.zero;
        controller.inputVec = Vector2.zero;
        controller.speed = 0f;
        skillController.SetDead(true);

        animator.ResetTrigger("HitTrigger");
        animator.ResetTrigger("DashTrigger");
        animator.ResetTrigger("AttackTrigger");

        animator.SetTrigger("DeadTrigger");

        StartCoroutine(DiePanel());
    }

    void Hit()
    {
        skillController.SetGettingHit(true);
        skillController.ResetAttack();

        animator.SetTrigger("HitTrigger");

        StartCoroutine(ClearGettingHitAfterDelay(0.4f));
    }

    IEnumerator DiePanel()
    {
        yield return new WaitForSeconds(1.5f);

        DamageTextManager.Instance.ShowDeath();

        yield return new WaitForSeconds(3.2f);

        SceneManager.LoadScene("Village");
    }

    private IEnumerator ClearGettingHitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        skillController.SetGettingHit(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class Player : MonoBehaviour, IDamagable
{
    
    public Animator animator;
    SpriteRenderer spriteRenderer;
       
    public PlayerStatus playerstatus;
    public ControllerPlayer controller;
    SkillController skillController;

    public bool isDead = false;

    [SerializeField] private float defenseFactor = 100f;
    [SerializeField] private float hpRegenRate = 2f;
    [SerializeField] private float mpRegenRate = 5f;

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

        RegenerateStats();
    }

    public void GetDamage(float damage)
    {
        float def = playerstatus.stats[StatType.DEF];
        float damageReductionRate = def / (defenseFactor + def);
        float finalDamage = damage * (1f - damageReductionRate);
        float hp = playerstatus.stats[StatType.CurrentHP] - finalDamage;

        playerstatus.SetStat(StatType.CurrentHP, hp);

        if (playerstatus.stats[StatType.CurrentHP] <= 0)
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

    private void RegenerateStats()
    {
        float dt = Time.deltaTime;

        //float currentHP = playerstatus.stats[StatType.CurrentHP];
        //float maxHP = playerstatus.stats[StatType.MaxHP];

        float currentMP = playerstatus.stats[StatType.CurrentMP];
        float maxMP = playerstatus.stats[StatType.MaxMP];

        //currentHP = Mathf.Min(currentHP + hpRegenRate * dt, maxHP);
        currentMP = Mathf.Min(currentMP + mpRegenRate * dt, maxMP);

        //playerstatus.SetStat(StatType.CurrentHP, currentHP);
        playerstatus.SetStat(StatType.CurrentMP, currentMP);
    }
}

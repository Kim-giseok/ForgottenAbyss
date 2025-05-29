using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamagable
{
    
    public Animator animator;
    SpriteRenderer spriteRenderer;
       
    public PlayerStatus playerstatus;
    public ControllerPlayer controller;
    SkillController skillController;

    public bool isDead = false;

    [SerializeField] private float defenseFactor = 100f;
    [SerializeField] private float hpRegenRate = 10f;
    [SerializeField] private float mpRegenRate = 5f;
    [SerializeField] private float speed;

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
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartCoroutine(TestGetDamage()); //테스트용
        }
#endif

        if (playerstatus.stats[StatType.CurrentHP] < playerstatus.stats[StatType.MaxHP] ||
        playerstatus.stats[StatType.CurrentMP] < playerstatus.stats[StatType.MaxMP])
        {
            RegenerateStats();
        }
    }

    public void GetDamage(float damage)
    {
        SoundManager.Instance.Playsfx("HitPlayer");
        // BoltsPool.Instance.CreateParticle(transform, "Hit2")
            // .SetSize(0.15f).SetColor(Color.yellow).SetPosition(transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.6f + Random.Range(-0.2f, 0.2f))).Play();

        
        float def = playerstatus.stats[StatType.DEF];
        float damageReductionRate = def / (defenseFactor + def);
        float finalDamage = damage * (1f - damageReductionRate);
        float hp = playerstatus.stats[StatType.CurrentHP] - finalDamage;

        playerstatus.SetStat(StatType.CurrentHP, hp);

        if (playerstatus.stats[StatType.CurrentHP] <= 0)
        {
            playerstatus.stats[StatType.CurrentHP] = 0;
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
        controller.isInvincible = true;
        controller.rigid.velocity = Vector2.zero;
        controller.inputVec = Vector2.zero;
        speed = controller.status.stats[StatType.SPEED];
        controller.status.stats[StatType.SPEED] = 0;
        skillController.SetDead(true);

        animator.ResetTrigger("HitTrigger");
        animator.ResetTrigger("DashTrigger");
        animator.ResetTrigger("AttackTrigger");

        animator.SetTrigger("DeadTrigger");

        StartCoroutine(DiePanel());
        
        // 외부 설정 초기화(추후 이동시키기)
        SoundManager.Instance.FadeOutBGM();
        LightManager.Instance.globalLight.intensity = 1;
        
        // 일괄 제거
        var enemies = FindObjectsOfType<EnemyController>();
        foreach (var eController in enemies) { eController.gameObject.SetActive(false); }
        BoltsPool.Instance.Clear();
    }

    void Hit()
    {
        skillController.SetGettingHit(true);
        skillController.ResetAttack();

        animator.SetTrigger("HitTrigger");

        StartCoroutine(ClearGettingHitAfterDelay(0.4f));
    }

    void revive()
    {
        isDead = false;
        controller.isAlive = true;
        controller.isInvincible = false;
        controller.rigid.velocity = Vector2.zero;
        controller.inputVec = Vector2.zero;
        controller.status.stats[StatType.SPEED] = speed;
        playerstatus.stats[StatType.CurrentHP] = playerstatus.stats[StatType.MaxHP];
    }

    IEnumerator DiePanel()
    {
        yield return new WaitForSeconds(1.5f);

        UIManager.Instance.ShowDeathUI();

        yield return new WaitForSeconds(3f);

        SceneLoader.Instance.LoadScene("Village");
        revive();
    }

    private IEnumerator ClearGettingHitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        skillController.SetGettingHit(false);
    }

    private void RegenerateStats()
    {
        float dt = Time.deltaTime;

        float currentHP = playerstatus.stats[StatType.CurrentHP];
        float maxHP = playerstatus.stats[StatType.MaxHP];

        float currentMP = playerstatus.stats[StatType.CurrentMP];
        float maxMP = playerstatus.stats[StatType.MaxMP];

        // MP 자동 회복
        if (currentMP < maxMP)
        {
            currentMP = Mathf.Min(currentMP + mpRegenRate * dt, maxMP);

            playerstatus.SetStat(StatType.CurrentMP, currentMP);
        }

        // 마을에서만 HP 자동 회복
        if (currentHP < maxHP && SceneManager.GetActiveScene().name == "Village")
        {
            {
                currentHP = Mathf.Min(currentHP + hpRegenRate * dt, maxHP);

                playerstatus.SetStat(StatType.CurrentHP, currentHP);
            }
        }
    }
}

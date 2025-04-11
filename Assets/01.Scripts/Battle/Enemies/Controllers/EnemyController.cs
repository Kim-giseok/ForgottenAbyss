using System;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyController : MonoBehaviour, IDamagable
{
    // SO로 추후 관리해도 좋을 듯
    [Header("Resource")]
    public float health;
    public float attack;

    // notice: 소환 기술을 위한 정보들
    public bool isAwake = true;
    [HideInInspector] public string SkillNodeName;

    public Rigidbody2D rigidbody { get; private set; }
    public Collider2D collider { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    
    public BTMachine btMachine { get; private set; }
    public EnemyAgent agent { get; private set; }
    public EnemyAnimationHandler animationHandler { get; private set; }
    public EnemyStatusHandler statusHandler { get; private set; }
    public EnemyRewardHandler rewardHandler { get; private set; }

    public RespawnArea respawnArea { get; private set; }
    

    private Transform pivot;
    public GameObject currWeapon; // 무기의 애니메이션이 발생할 수도 있음
    
    public void Awake()
    {
        agent = GetComponent<EnemyAgent>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        rewardHandler = GetComponent<EnemyRewardHandler>();

        animationHandler = new EnemyAnimationHandler(GetComponent<Animator>());
        statusHandler = new EnemyStatusHandler();
        btMachine = new(this);
    }
    
    public virtual void Init() {}

    public virtual void Start()
    {
        Debug.Log(isAwake);
        Init();
        
        try { MapSpawnManager.Instance.SpawnedMap.monsterManager.AddList(this); }
        catch { Debug.Log("there is no MapspawnManager"); }
        if (!isAwake) { PlayOneShot(); }
    }

    // character controller
    public void Flip(bool isFlip)
    {
        transform.rotation = Quaternion.Euler(0, isFlip ? 0 : 180, 0);
    }

    public void GetDamage(float damage) // notice: 잔상인 경우 제외
    {
        
        Vector3 textPosition = transform.position + Vector3.up * 1f;
        DamageTextManager.Instance.ShowDamage(textPosition, (int)damage);
        
        health -= damage;
        statusHandler.stamina -= 1;

        if (statusHandler.stamina <= 0)
        {
            statusHandler.stamina = 3;
            Debug.Log("knock out");
        }

        if (!statusHandler.isIgnoreHitAction)
        {
            statusHandler.isHit = true;
            btMachine.Notify();
        }

        if (health <= 0) Die();
    }

    private void FixedUpdate()
    {
        if (!isAwake) return;
        btMachine.Run();
    }

    private void OnAnimatedEvent(int value)
    {
        btMachine.OnAnimatedEvent(value == 1);
    }

    // 리워드 표시, 리스폰 아리어에서 제거
    public void Die()
    {
        try
        {
            MapSpawnManager.Instance.SpawnedMap.monsterManager.RemoveEnemy(this);
        }
        catch
        {
            Destroy(gameObject);
        }

        if (rewardHandler) { Instantiate(rewardHandler.GetRewardItem(), transform.position, Quaternion.identity); }
        
    }

    public void LookPlayer()
    {
        var direction = (agent.player.transform.position - transform.position).normalized;
        Flip(direction.x > 0);
    }

    public void PlayOneShot()
    {
        Debug.Log(1);
        // notice: 플레이어 이동이 필요한 경우
        agent.player.GetComponent<SpriteRenderer>().enabled = false;
        agent.player.GetComponent<Collider2D>().enabled = false;
        agent.player.GetComponent<Rigidbody2D>().gravityScale = 0;
        
        // collider.enabled = false; // notice: 환영인 경우 피격 무시 - 바닥 인식 안되는 현상 발생
        
        spriteRenderer.color = Color.black;
        
        gameObject.layer = LayerMask.NameToLayer("Player");
        var currNode = NodeManager.allNodes.Find(node => node.name == SkillNodeName).node;
        btMachine.Play(Activator.CreateInstance(currNode) as Node, () =>
        {
            // 조건에 따라 그 쪽으로 이동
            agent.player.transform.position = transform.position;
            
            // fix: 정리 필요 - 또한 몬스터가 사라지는 동안 오류 발생할 수 있음
            agent.player.GetComponent<SpriteRenderer>().enabled = true;
            agent.player.GetComponent<Collider2D>().enabled = true;
            agent.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            agent.player.GetComponent<Rigidbody2D>().gravityScale = 2f;

            
            Destroy(gameObject);
        });

        isAwake = true;
    }
}

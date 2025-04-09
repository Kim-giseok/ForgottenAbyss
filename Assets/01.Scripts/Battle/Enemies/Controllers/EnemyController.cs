using System;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamagable
{
    // SO로 추후 관리해도 좋을 듯
    [Header("Resource")]
    public float health;
    public float attack;

    public Rigidbody2D rigidbody { get; private set; }
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
        rewardHandler = GetComponent<EnemyRewardHandler>();

        animationHandler = new EnemyAnimationHandler(GetComponent<Animator>());
        statusHandler = new EnemyStatusHandler();
        btMachine = new(this);
    }

    public virtual void Start()
    {
        try
        {
            MapSpawnManager.Instance.SpawnedMap.monsterManager.AddList(this);
        }
        catch
        {
            Debug.Log("there is no MapspawnManager");
        }
    }

    // character controller
    public void Flip(bool isFlip)
    {
        transform.rotation = Quaternion.Euler(0, isFlip ? 0 : 180, 0);
    }

    public void GetDamage(float damage)
    {
        statusHandler.isHit = true;
        btMachine.Notify();
        health -= damage;

        if (health <= 0) Die();
    }

    private void FixedUpdate()
    {
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

        if (respawnArea != null)
        {
            Instantiate(rewardHandler.GetRewardItem(), transform.position, Quaternion.identity);
        }
    }
}

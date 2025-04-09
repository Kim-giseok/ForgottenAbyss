using System;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamagable
{
    [Header("Resource")]
    public float health;
    public float attack;

    public Rigidbody2D rigidbody { get; private set; }
    public BTMachine btMachine { get; private set; }
    public EnemyAgent agent { get; private set; }
    public EnemyAnimationHandler animationHandler { get; private set; }
    public EnemyStatusHandler statusHandler { get; private set; }

    public RespawnArea respawnArea { get; private set; }


    private Transform pivot;
    public GameObject currWeapon; // 무기의 애니메이션이 발생할 수도 있음

    public void Awake()
    {
        agent = GetComponent<EnemyAgent>();
        rigidbody = GetComponent<Rigidbody2D>();

        animationHandler = new EnemyAnimationHandler(GetComponent<Animator>());
        statusHandler = new EnemyStatusHandler();
        btMachine = new(this);
    }

    public virtual void Start()
    {
        try
        {
            MapSpawnManager.Instance.CurMap.monsterManager.AddList(this);
        }
        catch
        {
            Debug.Log("there is no MapspawnManager");
        }
        btMachine.Define(
                    new SelectorNode(
                        new SequenceNode(new HitNode()),
                        new SequenceNode(new TracingNode(), new AttackNode(), new CombatIdleNode(duration: 0.5f)),
                        new SequenceNode(new IdleNode(duration: 1), new PatrolNode(duration: 1)))
                );
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

        if (health <= 0)
        {
            try
            {
                MapSpawnManager.Instance.CurMap.monsterManager.RemoveEnemy(this);
            }
            catch
            {
                Destroy(gameObject);
            }
        }
    }

    // 리워드 표시, 리스폰 아리어에서 제거
    public void Clear()
    {

    }

    private void FixedUpdate()
    {
        btMachine.Run();
    }

    private void OnAnimatedEvent(int value)
    {
        btMachine.OnAnimatedEvent(value == 1);
    }
}

using System;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamagable
{
    [Header("Resource")]
    public float health;
    public float attack;

    [HideInInspector] public bool isHit;

    public BTMachine btMachine { get; private set; }
    public EnemyAgent agent { get; private set; }
    public Rigidbody2D rigidbody { get; private set; }
    
    
    private Transform pivot;
    public GameObject currWeapon; // 무기의 애니메이션이 발생할 수도 있음

    public void Awake()
    {
        agent = GetComponent<EnemyAgent>();
        rigidbody = GetComponent<Rigidbody2D>();
        
        btMachine = new(this);
    }

    private void Start()
    {
        pivot = transform.Find("Pivot");
        if (!pivot) { pivot = new GameObject("Pivot").transform; pivot.parent = transform; }
        
        btMachine.Define(
            new Selector(
                new Sequence(new HitNode()),
                new Sequence(new TracingNode(), new KnifeAttackNode()),
                new Sequence(new IdleNode(duration: 1), new PatrolNode(duration: 1)))
        );
    }

    public void Flip(bool isFlip)
    {
        transform.rotation = Quaternion.Euler(0, isFlip ? 0 : 180, 0);
    }
    
    public void GetDamage(float damage)
    {
        isHit = true;
        health -= damage;
        // btMachine.Notify();
        if(health <= 0) Destroy(gameObject);

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

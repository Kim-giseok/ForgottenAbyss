using System;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamagable
{
    [Header("Resource")]
    public float health;
    public float attack;

    public BTMachine btMachine { get; private set; }
    public EnemyAgent agent { get; private set; }
    public Rigidbody2D rigidbody { get; private set; }
    
    
    private Transform pivot;
    public WeaponSO currWeapon;

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

        GameObject weapon = new("Weapon");
        weapon.AddComponent<SpriteRenderer>().sprite = currWeapon.image;
        weapon.transform.SetParent(pivot);

        btMachine.Define(
            new Selector(
                // new Sequence(new TracingNode(), new KnifeAttackNode()),
                new Sequence(new IdleNode(duration: 1), new PatrolNode(duration: 1)))
        );
    }

    public void Flip(bool isFlip)
    {
        transform.rotation = Quaternion.Euler(0, isFlip ? 0 : 180, 0);
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;
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

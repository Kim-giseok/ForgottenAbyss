using UnityEngine;

public class EnemyBaseController: MonoBehaviour
{
    public BTMachine btMachine { get; protected set; }
    public Transform target { get; protected set; }
    
    public Collider2D collider { get; protected set; }
    public Rigidbody2D rigidbody { get; protected set; }
    public SpriteRenderer spriteRenderer { get; protected set; }
    public EnemyAnimationHandler animationHandler { get; protected set; }
    
    public EnemyCombatHandler combatHandler { get; protected set; }

    protected virtual void Awake()
    {
        btMachine = new(this);

        collider = GetComponent<Collider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        animationHandler = new EnemyAnimationHandler(GetComponent<Animator>());

        combatHandler = new EnemyCombatHandler();
    }
    
    protected void FixedUpdate()
    {
        btMachine.Run();
    }

    protected void OnAnimatedEvent(int value) // notice: string과 enum으로 좀 더 다양하게 구현하도록 처리
    {
        btMachine.OnAnimatedEvent(value == 1);
    }

    protected void OnASD(bool isWalkable)
    {
    }

    // character controller //
    public void Flip(bool isFlip)
    {
        transform.rotation = Quaternion.Euler(0, isFlip ? 0 : 180, 0);
    }
    public void LookTarget()
    {
        var direction = (target.position - transform.position).normalized;
        Flip(direction.x > 0);
    }

}
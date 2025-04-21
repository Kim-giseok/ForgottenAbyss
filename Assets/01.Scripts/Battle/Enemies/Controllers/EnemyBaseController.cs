using UnityEngine;

public class EnemyBaseController: MonoBehaviour
{
    public EnemyAgent agent { get; private set; }
    public BTMachine machine { get; protected set; }
    public Collider2D collider { get; protected set; }
    public Rigidbody2D rigidbody { get; protected set; }
    public SpriteRenderer spriteRenderer { get; protected set; }
    public Material material { get; protected set; }
    public EnemyAnimationHandler animnHandler { get; protected set; }
    public EnemyCombatHandler combatHandler { get; protected set; }
    public EnemyDetectHandler detectHandler { get; protected set; }
    
    
    public float animationValue = 1; // 애니메이터에서 직접 컨트롤 가능

    protected virtual void Awake()
    {
        machine = new(this);

        collider = GetComponent<Collider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        material = spriteRenderer.material;
        animnHandler = GetComponent<EnemyAnimationHandler>();

        combatHandler = new EnemyCombatHandler();
        detectHandler = GetComponent<EnemyDetectHandler>();
        
        // summon에선 없도록 처리
        agent = GetComponent<EnemyAgent>(); // 플레이어의 경우 주면 몬스터를 찾도록(혹은 새 클래스로 분리하기)
    }
    
    protected void FixedUpdate()
    {
        material.SetFloat("_YValue", animationValue);
        machine.Run();
    }

    // Destory된 이후에 발생한 경우 오류 발생
    protected void OnAnimatedEvent(int value) // notice: string과 enum으로 좀 더 다양하게 구현하도록 처리
    {
        machine.OnAnimatedEvent(value == 1);
    }

    public void OnDetected(EnemyDetectHandler.DetectType detectType, bool value)
    {
        machine.OnDetected(detectType, value);
    }

    public void OnAgentDetected(EnemyAgent.Status status)
    {
        machine.OnAgentDetected(status);
    }

    // character controller //
    public void Flip(bool isFlip)
    {
        transform.rotation = Quaternion.Euler(0, isFlip ? 0 : 180, 0);
    }
    public void LookTarget()
    {
        Flip(agent.GetDirection().x > 0);
    }

}
using UnityEngine;

public class EnemyBaseController: MonoBehaviour
{
    public EnemyAgent agent { get; private set; }
    public BTMachine machine { get; protected set; }
    public CapsuleCollider2D Collider { get; protected set; }
    public Rigidbody2D Rigidbody { get; protected set; }
    public SpriteRenderer Renderer { get; protected set; }
    public Material material { get; protected set; }
    public EnemyAnimHandler animHandler { get; protected set; }
    public EnemyCombatHandler combatHandler { get; protected set; }
    public EnemyDetectHandler detectHandler { get; protected set; }
    public EnemySoundHandler soundHandler { get; protected set; }

    public EnemyBoard board { get; protected set; }
    

    protected virtual void Awake()
    {
        machine = new(this);

        Collider = GetComponent<CapsuleCollider2D>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>(); 
        material = Renderer.material;
        animHandler = GetComponent<EnemyAnimHandler>();

        combatHandler = new EnemyCombatHandler();
        detectHandler = GetComponent<EnemyDetectHandler>();
        soundHandler = GetComponent<EnemySoundHandler>();
        board = new EnemyBoard();
        
        // summon에선 없도록 처리
        agent = GetComponent<EnemyAgent>(); // 플레이어의 경우 주면 몬스터를 찾도록(혹은 새 클래스로 분리하기)
    }
    
    protected void FixedUpdate()
    {
        machine.Run();
    }

    // Destory된 이후에 발생한 경우 오류 발생
    protected void OnAnimatedEvent(int value) // notice: string과 enum으로 좀 더 다양하게 구현하도록 처리
    {
        machine.OnAnimatedEvent(value == 1);
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
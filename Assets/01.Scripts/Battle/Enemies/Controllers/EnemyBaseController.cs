using UnityEngine;

public class EnemyBaseController: MonoBehaviour
{
    public EnemyAgent Agent { get; private set; }
    public BTMachine Machine { get; protected set; }
    public CapsuleCollider2D Collider { get; protected set; }
    public Rigidbody2D Rigid { get; protected set; }
    public SpriteRenderer Render { get; protected set; }
    public Material Mat { get; protected set; }
    public EnemyAnimHandler Anim { get; protected set; }
    public EnemyCombatHandler combatHandler { get; protected set; }
    public EnemyDetectHandler detectHandler { get; protected set; }
    public EnemySoundHandler soundHandler { get; protected set; }

    public EnemyBoard Board { get; protected set; }
    

    protected virtual void Awake()
    {
        Machine = new(this);

        Collider = GetComponent<CapsuleCollider2D>();
        Rigid = GetComponent<Rigidbody2D>();
        Render = GetComponent<SpriteRenderer>(); 
        Mat = Render.material;
        Anim = GetComponent<EnemyAnimHandler>();

        combatHandler = new EnemyCombatHandler();
        detectHandler = GetComponent<EnemyDetectHandler>();
        soundHandler = GetComponent<EnemySoundHandler>();
        Board = new EnemyBoard();
        
        // summon에선 없도록 처리
        Agent = GetComponent<EnemyAgent>(); // 플레이어의 경우 주면 몬스터를 찾도록(혹은 새 클래스로 분리하기)
    }
    
    protected void FixedUpdate()
    {
        Machine.Run();
    }

    // Destory된 이후에 발생한 경우 오류 발생
    protected void OnAnimatedEvent(int value) // notice: string과 enum으로 좀 더 다양하게 구현하도록 처리
    {
        Machine.OnAnimatedEvent(value == 1);
    }
    
    // character controller //
    // notice: 플립 개념이 거꾸로 되어있다.
    public void Flip(bool isFlip)
    {
        transform.rotation = Quaternion.Euler(0, isFlip ? 0 : 180, 0);
    }
    public void LookTarget()
    {
        if(Agent) { Flip(Agent.GetDirection().x > 0); }
    }

}
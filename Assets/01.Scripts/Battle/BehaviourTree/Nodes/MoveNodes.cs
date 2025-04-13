using UnityEngine;

// notice: 단순 딜레이이며 조건 체크만 분리 필요
public class IdleNode : Node
{
    private float duration;

    public IdleNode(float duration)
    {
        this.duration = duration;
    }

    public override void Start()
    {
        controller.rigidbody.velocity = new Vector2(0, controller.rigidbody.velocity.y);
    }

    public override void Update()
    {
        Debug.Log("idle");
        if (currTime >= duration) { SetStatus(Status.Success); }
    }
}

// do: 범위를 지정해두는 방법이 있음
public class PatrolMove : Node
{
    private float duration;

    public PatrolMove(float duration) { this.duration = duration; }

    public override void Start()
    {
        Vector2 direction = Random.Range(0, 10) <= 5 ? Vector2.left : Vector2.right;
        context.Set("direction", direction);
        controller.Flip(direction == Vector2.right);
        
        if (!controller.detectHandler.isWalkable) { SetStatus(Status.Fail); return; }
        controller.animationHandler.Set(EnemyAnimationHandler.Run, true);
    }

    public override void Update()
    {
        Debug.Log(currTime);
        if (currTime >= duration) { SetStatus(Status.Success); return; }
        
        controller.rigidbody.velocity = new Vector2(context.Get<Vector2>("direction").x, controller.rigidbody.velocity.y);
    }
    
    // public override void OnPhysicsDetected(EnemyDetectHandler.DetectType detectType, string value)
    // {
    //     if (detectType == EnemyDetectHandler.DetectType.Walkable && !bool.Parse(value))
    //     {
    //         SetStatus(Status.Success);
    //     }
    // }
    //
    //
    // public override void End()
    // {
    //     controller.animationHandler.Set(EnemyAnimationHandler.Run, false);
    // }
}

public class TracingNode : Node
{
    public override void Start()
    {
        if(controller.agent.status == EnemyAgent.Status.Tracked) { SetStatus(Status.Success); return; } // notice: 애초에 한번 더 분류하는 게 맞지 않을까?
        controller.animationHandler.Set(EnemyAnimationHandler.Run, true);
    }
    
    public override void Update()
    {
        controller.LookTarget();
        controller.rigidbody.velocity = new Vector2(controller.agent.GetDirection().x * controller.agent.tracingSpeed, controller.rigidbody.velocity.y);
    }
    
    public override void End()
    {
        controller.animationHandler.Set(EnemyAnimationHandler.Run, false);
    }

    public override void OnAgentDetected(EnemyAgent.Status status)
    {
        if(status == EnemyAgent.Status.Tracked) { SetStatus(Status.Success); return; }
    }
}

public class DashNode : Node // 현재 방향이거나 타겟 방향
{
    private int direction;
    public override void Start()
    {
        // 방향 계산 개념이 들어감, 이동에서 처리해줘야 하는 부분
        // var currDirection = (controller.target.transform.position - controller.transform.position).normalized;
        // direction = currDirection.x < 0 ? -1 : 1;
        
        controller.Flip(direction == 1);
    }

    public override void Update()
    {
        controller.rigidbody.velocity = new Vector2(3f, controller.rigidbody.velocity.y);
    }
}

// summon인 경우 위치 체크 문제 발생
public class JumpNode : Node
{
    public override void Start()
    {
        controller.animationHandler.Set(EnemyAnimationHandler.Attack);
        // var currDirection = (controller.target.position - controller.transform.position).normalized;
        // var direction = currDirection.x < 0 ? -1 : 1; // 타깃이 몬스터라면 문제가 생김
        
        // controller.rigidbody.AddForce(new Vector2(direction * 1f, 4f), ForceMode2D.Impulse);
        controller.rigidbody.AddForce(new Vector2(1f, 4f), ForceMode2D.Impulse);
    }
}

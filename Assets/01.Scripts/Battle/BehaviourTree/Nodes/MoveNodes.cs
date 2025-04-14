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
        
        controller.animationHandler.Play("Idle");
    }

    public override void Update()
    {
        if (currTime >= duration) { SetStatus(Status.Success); }
    }

    public override void OnAgentDetected(EnemyAgent.Status status)
    {
        if(status != EnemyAgent.Status.None) { SetStatus(Status.Fail); }
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
        
        // 단일인 경우 문제 발생
        // Debug.Log(controller.detectHandler.isWalkable);
        if (!controller.detectHandler.isWalkable)
        {
            controller.Flip(direction != Vector2.right);
            SetStatus(Status.Fail); return;
        } 
     
        controller.animationHandler.Play("Run");
    }

    public override void Update()
    {
        if (currTime >= duration)
        {
            SetStatus(Status.Success); return;
        }
        
        controller.rigidbody.velocity = new Vector2(context.Get<Vector2>("direction").x, controller.rigidbody.velocity.y);
    }
    
    public override void OnPhysicsDetected(EnemyDetectHandler.DetectType detectType, string value)
    {
        if (detectType == EnemyDetectHandler.DetectType.Walkable && !bool.Parse(value))
        {
            SetStatus(Status.Success);
        }
    }
    
    public override void OnAgentDetected(EnemyAgent.Status status)
    {
        if(status != EnemyAgent.Status.None) { SetStatus(Status.Fail); }
    }
}

public class TracingNode : Node
{
    public override void Start()
    {
        // 추적이 완료되면 무한 재귀 발생
        if(controller.agent.status == EnemyAgent.Status.None) { SetStatus(Status.Fail); return; }
        if(controller.agent.status == EnemyAgent.Status.Tracked) { SetStatus(Status.Success); return; }
        
        controller.animationHandler.Play("Run");
    }
    
    public override void Update()
    {
        controller.LookTarget();
        // bug: 추적 방향이 위쪽이면 속도까지 줄어드는 문제 발생
        controller.rigidbody.velocity = new Vector2(controller.agent.GetDirection().x * controller.agent.tracingSpeed, controller.rigidbody.velocity.y);
    }
    
    public override void OnAgentDetected(EnemyAgent.Status status)
    {
        if(status == EnemyAgent.Status.None) { SetStatus(Status.Fail); }
        if(status == EnemyAgent.Status.Tracked) { SetStatus(Status.Success); }
    }
}

public class DashNode : Node // 현재 방향이거나 타겟 방향
{
    private int direction;
    public override void Start()
    {
    }

    public override void Update()
    {
        if(currTime > 1) { SetStatus(Status.Success); return; }
        controller.rigidbody.velocity = new Vector2(controller.transform.localEulerAngles.y == 180 ? -2 : 2, controller.rigidbody.velocity.y);
    }
}

// summon인 경우 위치 체크 문제 발생
public class JumpNode : Node
{
    public override void Start()
    {
        // controller.animationHandler.Set(EnemyAnimationHandler.Attack);
        // var currDirection = (controller.target.position - controller.transform.position).normalized;
        // var direction = currDirection.x < 0 ? -1 : 1; // 타깃이 몬스터라면 문제가 생김
        
        // controller.rigidbody.AddForce(new Vector2(direction * 1f, 4f), ForceMode2D.Impulse);
        controller.rigidbody.AddForce(new Vector2(1f, 4f), ForceMode2D.Impulse);
    }
}

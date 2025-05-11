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
        controller.Rigidbody.velocity = new Vector2(0, controller.Rigidbody.velocity.y);
        controller.animnHandler.Play("Idle");
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
        
        if (!controller.detectHandler.isWalkable)
        {
            controller.Flip(direction != Vector2.right);
            SetStatus(Status.Fail); return;
        } 
     
        controller.animnHandler.Play("Run");
    }

    public override void Update()
    {
        if (currTime >= duration)
        {
            SetStatus(Status.Success); return;
        }
        
        controller.Rigidbody.velocity = new Vector2(context.Get<Vector2>("direction").x, controller.Rigidbody.velocity.y);
    }
    
    public override void OnAgentDetected(EnemyAgent.Status status)
    {
        if(status != EnemyAgent.Status.None) { SetStatus(Status.Fail); }
    }
}

// Move 노드로 해소하기
public class TracingNode : Node
{
    public override void Start()
    {
        // 추적이 완료되면 무한 재귀 발생
        if(controller.agent.status == EnemyAgent.Status.None) { SetStatus(Status.Fail); return; }
        if(controller.agent.status == EnemyAgent.Status.Tracked) { SetStatus(Status.Success); return; }
        controller.animnHandler.Play("Run");
    }
    
    public override void Update()
    {
        // 플랫폼을 우선 체크하여 넘어갈 수 없는 상황이라면 점프
        
        controller.LookTarget();
        // bug: 추적 방향이 위쪽이면 속도까지 줄어드는 문제 발생
        
        // NavSurface.Instance.targetPlatforms.ContainsKey(controller.agent.target)
        
        // if (NavSurface.Instance.targetPlatforms[controller.agent.target] !=
        //     NavSurface.Instance.targetPlatforms[controller.gameObject])
        // {
        //     // 플랫폼 이동과 추적 간의 순서는 좀 더 생각해보기
        //     SetStatus(Status.Success);            
        // }
        
        controller.Rigidbody.velocity = new Vector2(controller.agent.GetDirection().x * controller.agent.tracingSpeed, controller.Rigidbody.velocity.y);
    }
    
    public override void OnAgentDetected(EnemyAgent.Status status)
    {
        if(status == EnemyAgent.Status.None) { SetStatus(Status.Fail); }
        if(status == EnemyAgent.Status.Tracked) { SetStatus(Status.Success); }
    }

    // public override void OnPhysicsDetected(EnemyDetectHandler.DetectType detectType, bool able)
    // {
    //     if (detectType == EnemyDetectHandler.DetectType.Blocked && able)
    //     {
    //         controller.Rigidbody.AddForce(Vector2.up * 6, ForceMode2D.Impulse); // 높이가 달라지면?
    //     }
    // }
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
        controller.Rigidbody.AddForce(new Vector2(1f, 4f), ForceMode2D.Impulse);
    }
}

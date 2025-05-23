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
        controller.Rigid.velocity = new Vector2(0, controller.Rigid.velocity.y);
        controller.Anim.Play("Idle");
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
     
        controller.Anim.Play("Run");
    }

    public override void Update()
    {
        if (currTime >= duration)
        {
            SetStatus(Status.Success); return;
        }
        
        controller.Rigid.velocity = new Vector2(context.Get<Vector2>("direction").x, controller.Rigid.velocity.y);
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
        // if(controller.Agent.status == EnemyAgent.Status.None) { SetStatus(Status.Fail); return; }
        // if(controller.Agent.status == EnemyAgent.Status.Tracked) { SetStatus(Status.Success); return; }
        controller.Anim.Play("Run");
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
        
        // SO에 있는 이동 데이터로 전달하기
        controller.Rigid.velocity = new Vector2(controller.Agent.GetDirection().x * controller.Agent.TracingSpeed, controller.Rigid.velocity.y);
    }

    // public override void OnPhysicsDetected(EnemyDetectHandler.DetectType detectType, bool able)
    // {
    //     if (detectType == EnemyDetectHandler.DetectType.Blocked && able)
    //     {
    //         controller.Rigidbody.AddForce(Vector2.up * 6, ForceMode2D.Impulse); // 높이가 달라지면?
    //     }
    // }
}


public class MovePlatformNode : Node
{
    public override void Start()
    {
        if (NavSurface.Instance.GetPlatformId(controller.Agent.target) == NavSurface.Instance.GetPlatformId(controller.gameObject))
        {
            // 플랫폼 이동과 추적 간의 순서는 좀 더 생각해보기
            SetStatus(Status.Success);
            return;
        }
        
        var targetPlatform = NavSurface.Instance.platforms.Find(platform => platform.id == NavSurface.Instance.GetPlatformId(controller.Agent.target));
        Vector2 destination = targetPlatform.centerCell.WorldPos;
        context.Set("destination", new Vector3(destination.x, destination.y, 0));
        
        controller.Collider.isTrigger = true;
        controller.Rigid.gravityScale = 0;
        // controller.rigidbody.isKinematic = true;
    }

    public override void Update()
    {
        Vector3 destination = context.Get<Vector3>("destination") + Vector3.up;
        // 박싱으로 인한 성능 문제 예상해보기
        Vector3 direction = (destination - controller.transform.position).normalized;
        controller.transform.position += direction * (Time.deltaTime * 6f);  // 이동

        // 목표에 거의 도달하면 완료
        if (Vector3.Distance(destination, controller.transform.position) < 0.5f)
        {
            // 중앙까지는 잘 도착햇지만 다시 플레이어 추적을 해야해서 에러남
            SetStatus(Status.Success);
        }
    }

    public override void End()
    {
        controller.Collider.isTrigger = false;
        // controller.rigidbody.isKinematic = false;
        controller.Rigid.gravityScale = 2;
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
        controller.Rigid.velocity = new Vector2(controller.transform.localEulerAngles.y == 180 ? -2 : 2, controller.Rigid.velocity.y);
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
        controller.Rigid.AddForce(new Vector2(1f, 4f), ForceMode2D.Impulse);
    }
}

using Unity.VisualScripting;
using UnityEngine;

// notice: 단순 딜레이이며 조건 체크만 분리 필요
public class IdleNode : Node
{
    private float currTime;
    public float duration;

    public IdleNode(float duration)
    {
        this.duration = duration;
    }

    public override void Start()
    {
        currTime = 0; // notice 시간 자체를 BT에서 가져도 될 듯
        controller.rigidbody.velocity = new Vector2(0, controller.rigidbody.velocity.y);
    }

    public override void Update()
    {
        Vector2 distance = (controller.agent.player.transform.position - controller.transform.position);
        
        if (controller.agent.detectedDistance >= distance.magnitude)
        {
            SetStatus(Status.Fail);
            return;
        }
        
        currTime += Time.deltaTime;
        if (currTime >= duration)
        {
            SetStatus(Status.Success);
            return;
        }
    }
}

// do: 범위를 지정해두는 방법이 있음
public class PatrolNode : Node
{
    private float currTime; // notice: 공통으로 사용되는 경우 많음 - 차라리 BT에서 경과 시간을 가질 수 도 있을 듯
    public float duration;
    private Vector2 direction;

    public PatrolNode(float duration)
    {
        this.duration = duration;
    }

    public override void Start()
    {
        currTime = 0;
        direction = Random.Range(0, 10) <= 5 ? Vector2.left : Vector2.right;

        controller.Flip(direction == Vector2.right);
        // fix: 갈 수 있는 곳 인지 체크 필요
        controller.animationHandler.Set(EnemyAnimationHandler.Run, true);
    }

    public override void Update()
    {
        Vector2 distance = (controller.agent.player.transform.position - controller.transform.position);
        
        if (controller.agent.detectedDistance >= distance.magnitude)
        {
            SetStatus(Status.Fail);
            return;
        }
        
        currTime += Time.deltaTime;
        if (currTime >= duration)
        {
            SetStatus(Status.Success);
            return;
        }
        
        controller.rigidbody.velocity = new Vector2(direction.x, controller.rigidbody.velocity.y);
    }

    public override void End()
    {
        controller.animationHandler.Set(EnemyAnimationHandler.Run, false);
    }
}

public class TracingNode : Node
{
    public override void Start()
    {
        // fix: 중복 코드 발생하는 부분 해결 필요
        Vector2 distance = (controller.agent.player.transform.position - controller.transform.position);
        
        if (controller.agent.stoppingDistance > distance.magnitude) // 바로 공격으로 진입
        {
            SetStatus(Status.Success);
            return;
        }
        
        controller.animationHandler.Set(EnemyAnimationHandler.Run, true);
    }
    
    public override void Update()
    {
        Vector2 distance = (controller.agent.player.transform.position - controller.transform.position);
        
        if (controller.agent.detectedDistance < distance.magnitude)
        {
            SetStatus(Status.Fail);
            return;
        }
        
        if (controller.agent.stoppingDistance > distance.magnitude)
        {
            SetStatus(Status.Success);
           return;
        }
        
        controller.Flip(distance.normalized.x > 0);
        controller.rigidbody.velocity = new Vector2(distance.normalized.x * controller.agent.tracingSpeed, controller.rigidbody.velocity.y);
    }
    
    public override void End()
    {
        controller.animationHandler.Set(EnemyAnimationHandler.Run, false);
    }
}

// 무기가 Node를 결정할 수 있도록
public class AttackNode : Node
{
    public override void Start()
    {
        controller.animationHandler.Set(EnemyAnimationHandler.Attack);
    }

    public override void OnAnimatedEvent(bool isFire)
    {
        if (isFire)
        {
            ProjectileManager.Instance.CreateMeleeProjectile(controller.transform, controller.attack);
        }
        else
        {
            ProjectileManager.Instance.DestroyMeleeProjectile(controller.transform);
        }
    }

    // fix: 의미 없는 호출이 발생할 수 있느 점에 대한 고려 필요
    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Attack")) return;
        if(status == AnimationStatus.End) SetStatus(Status.Success);
    }


    public override void End() // notice: 공격 중 피격 당하는 경우
    {
        ProjectileManager.Instance.DestroyMeleeProjectile(controller.transform);
    }
}

public class RangeAttackNode : Node
{
    public override void Start()
    {
        controller.animationHandler.Set(EnemyAnimationHandler.Attack);
    }

    public override void OnAnimatedEvent(bool isFire)
    {
        if (isFire)
        {
            ProjectileManager.Instance.CreateProjectile(controller.transform, controller.attack);
        }
    }


    public override void End()
    {
    }
}

public class RangeToTargetNode : Node
{
    public override void Start()
    {
        controller.animationHandler.Set(EnemyAnimationHandler.Attack);
    }

    public override void OnAnimatedEvent(bool isFire)
    {
        if (isFire)
        {
            ProjectileManager.Instance.CreateProjectile(controller.transform, controller.attack, degree: ProjectileManager.Instance.GetDegreeByDirection((controller.agent.player.transform.position - controller.transform.position).normalized));
        }
        else
        {
            SetStatus(Status.Success);
        }
    }


    public override void End()
    {
    }
}

// notice: Idle과 같은 상황 - 특수 목적 필요
public class CombatIdleNode : Node 
{
    private float currTime;
    public float duration;

    public CombatIdleNode(float duration)
    {
        this.duration = duration;
    }

    public override void Start()
    {
        currTime = 0;
    }

    public override void Update()
    {
        controller.LookPlayer();
        
        currTime += Time.deltaTime;
        if (currTime >= duration)
        {
            SetStatus(Status.Success);
        }
    }
}

public class GuardNode : Node
{
    public override void Start()
    {
        controller.statusHandler.isDefense = true;
    }

    public override void End()
    {
        controller.statusHandler.isDefense = false;
    }
}

// knockBack이 들어갈 수도 있도록
public class HitNode : Node
{
    private bool isNockBack = false;
    
    public override void Start()
    {
        // notice: status - GOAP 를 위해 존재할 예정
        // 조건 자체가 맨 앞에서 체크 필요
        if (!controller.statusHandler.isHit) // do: 시퀀스를 통행 애초에 진입이 안되도록 노드 지정
        {
            SetStatus(Status.Fail);
            return;
        }
        
        // 타격 받은 쪽으로 회전
        Vector2 direction = (controller.agent.player.transform.position - controller.transform.position).normalized;
        controller.Flip(direction.x > 0);
        
        controller.animationHandler.Set(EnemyAnimationHandler.Hit);
        controller.statusHandler.isHit = false;
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Hit")) return;
        if (status == AnimationStatus.End) SetStatus(Status.Success);
    }
}

public class DieNode : Node
{
    public override void Start()
    {
        controller.Die();
    }
}
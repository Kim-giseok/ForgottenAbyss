using Unity.VisualScripting;
using UnityEngine;

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
        currTime = 0;
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

public class PatrolNode : Node
{
    private float currTime;
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
            SetStatus(Status.Success);
        }
    }

    public override void OnAnimated(AnimationStatus status, Animator animator)
    {
        // notice: 시작된 후 애니메이션이 변경되면서 바로 인식되는 문제 발생
        if (status == AnimationStatus.End)
        {
        }
    }

    public override void End()
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

    public override void OnAnimated(AnimationStatus status, Animator animator) // 우선 인식되는 현상 발생
    {
        
    }

    public override void End()
    {
    }
}

public class RangeToTargetNode : Node
{

    public float GetDegreeToTarget()
    {
        Vector2 direction = (controller.agent.player.transform.position - controller.transform.position).normalized;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 날라가는 방향 계산
    }
    
    public override void Start()
    {
        controller.animationHandler.Set(EnemyAnimationHandler.Attack);
    }

    public override void OnAnimatedEvent(bool isFire)
    {
        if (isFire)
        {
            
            ProjectileManager.Instance.CreateProjectile(controller.transform, controller.attack, degree: GetDegreeToTarget());
        }
        else
        {
            SetStatus(Status.Success);
        }
    }

    public override void OnAnimated(AnimationStatus status, Animator animator) // 우선 인식되는 현상 발생
    {
        
    }

    public override void End()
    {
    }
}

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
        currTime += Time.deltaTime;
        if (currTime >= duration)
        {
            SetStatus(Status.Success);
        }
    }
}

// knockBack이 들어갈 수도 있도록
public class HitNode : Node
{
    public override void Start()
    {
        // status - GOAP 를 위해 존재할 예정
        if (!controller.statusHandler.isHit) 
        {
            SetStatus(Status.Fail);
            return;
        }
        
        Vector2 direction = (controller.agent.player.transform.position - controller.transform.position).normalized;
        controller.Flip(direction.x > 0);
        
        controller.animationHandler.Set(EnemyAnimationHandler.Hit);
        
        controller.statusHandler.isHit = false;
        SetStatus(Status.Success);
    }

    public override void OnAnimated(AnimationStatus status, Animator animator)
    {
        if (status == AnimationStatus.End)
        {
        }
    }
}

public class DieNode : Node
{
    public override void Start()
    {
        controller.Die();
    }
}
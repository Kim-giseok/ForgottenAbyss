using UnityEngine;

public class DashAttack : Node
{
    private int direction;
    public override void Start()
    {
        controller.animationHandler.Set(EnemyAnimationHandler.Attack);
        var currDirection = (controller.agent.player.transform.position - controller.transform.position).normalized;
        this.direction = currDirection.x < 0 ? -1 : 1;
        Physics2D.IgnoreCollision(controller.collider, controller.agent.player.transform.GetComponent<Collider2D>(), true);
    }

    public override void Update()
    {
        // notice: 실제 다른 점
        // 방향 체크 로직으로 controller에서 공통으로 가지면 좋을 듯
        controller.rigidbody.velocity = new Vector2(direction * 3f, controller.rigidbody.velocity.y);
    }

    public override void OnAnimatedEvent(bool isFire)
    {
        // 거의 유사한 부분 그러나 스킬에 따라 프로젝타일이 달라지거나 하는 등의 방식이 생길 수 있음
        if (isFire)
        {
            // ProjectileManager.Instance.CreateProjectile(controller.transform, 10);
        }
        else
        {
        }
    }

    // notice: 역시 동일한 부분 상속 가능
    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Attack")) return;
        

        if (status == AnimationStatus.Start)
        {
            ProjectileManager.Instance.CreateMeleeProjectile(controller.transform, 10);
        }

        if (status == AnimationStatus.End)
        {
            Physics2D.IgnoreCollision(controller.collider, controller.agent.player.transform.GetComponent<Collider2D>(), false);
            ProjectileManager.Instance.DestroyMeleeProjectile(controller.transform);
            SetStatus(Status.Success);
        }
    }
}

public class JumpAttack : Node
{
    private int direction;
    public override void Start()
    {
        controller.animationHandler.Set(EnemyAnimationHandler.Attack);
        var currDirection = (controller.agent.player.transform.position - controller.transform.position).normalized;
        this.direction = currDirection.x < 0 ? -1 : 1;
        
        Physics2D.IgnoreCollision(controller.collider, controller.agent.player.transform.GetComponent<Collider2D>(), true);
        controller.rigidbody.AddForce(new Vector2(direction * 1f, 4f), ForceMode2D.Impulse);
    }


    // notice: 직접적인 공격은 시작부터 끝인 경우가 많음
    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Attack")) return;
        

        if (status == AnimationStatus.Start)
        {
            ProjectileManager.Instance.CreateMeleeProjectile(controller.transform, 10);
        }

        if (status == AnimationStatus.End)
        {
            Physics2D.IgnoreCollision(controller.collider, controller.agent.player.transform.GetComponent<Collider2D>(), false);
            ProjectileManager.Instance.DestroyMeleeProjectile(controller.transform);
            
            ProjectileManager.Instance.CreateProjectile(controller.transform, controller.attack, attrs: new []{ new ParabolaAttr(new Vector2(1, 2), 2)}, degree: -100, index: 2);
            ProjectileManager.Instance.CreateProjectile(controller.transform, controller.attack, attrs: new []{ new ParabolaAttr(new Vector2(-1, 2), 2)}, degree: -100, index: 2);

            SetStatus(Status.Success);
        }
    }

    public override void End() // 애니메이션 끝나지 않았는데 공격 당하는 경우
    {
        ProjectileManager.Instance.DestroyMeleeProjectile(controller.transform);
    }
}

// 동시 다발적으로 발사하는 경우
public class ParallelShotNode : Node
{
    private float currTime;
    public float duration;
    private Vector2 direction;

    public ParallelShotNode()
    {
        // this.duration = duration;
    }

    public override void Start()
    {
        currTime = 0;
        direction = Random.Range(0, 10) <= 5 ? Vector2.left : Vector2.right;

        controller.Flip(direction == Vector2.right);
        controller.animationHandler.Set(EnemyAnimationHandler.Run, true);

        for (int degree = 0; degree < 360; degree += 40)
        {
            ProjectileManager.Instance.CreateProjectile(controller.transform, controller.attack, attrs: new []{ new ReflectAttr()}, degree: degree, index: 3);
            // ProjectileManager.Instance.CreateProjectile(controller.transform, controller.attack, attrs: new []{ new StraightAttr()}, degree: degree, index: 3);
        }

    }

    public override void Update()
    {
        Vector2 distance = (controller.agent.player.transform.position - controller.transform.position);
    
        currTime += Time.deltaTime;
        if (currTime >= 2f)
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

// 순차적으로 발생하는 경우
public class SequentialShotNode : Node { }

public class OneShotNode : Node
{
    
}
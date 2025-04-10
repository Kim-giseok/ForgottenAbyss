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
            SetStatus(Status.Success);
        }
    }
}

// 동시 다발적으로 발사하는 경우
public class ParallelShotNode: Node {}

// 순차적으로 발생하는 경우
public class SequentialShotNode : Node { }

public class OneShotNode : Node
{
    
}
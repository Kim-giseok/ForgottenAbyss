
using UnityEngine;

public class FlyingIdleNode : Node
{
    private float currTime;
    public float duration;

    public FlyingIdleNode(float duration)
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
        currTime += Time.deltaTime;
        if (currTime >= duration)
        {
            SetStatus(Status.Success);
            return;
        }
    }
}

// 공격
public class FlyingPatrolAttackNode : Node
{
    private float currTime;
    public float duration;
    private Vector2 direction;

    public FlyingPatrolAttackNode(float duration)
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
        
        currTime += Time.deltaTime;
        if (currTime >= duration)
        {
            SetStatus(Status.Success);
            return;
        }
        
        controller.rigidbody.velocity = new Vector2(direction.x, controller.rigidbody.velocity.y);
    }

    public override void OnAnimatedEvent(bool isFire)
    {
        if (isFire)
        {
            ProjectileManager.Instance.CreateProjectile(controller.transform, controller.attack, attrs: new []{ new StraightAttr()}, degree: -90, index: 2);
            ProjectileManager.Instance.CreateProjectile(controller.transform, controller.attack, attrs: new []{ new StraightAttr()}, degree: -80, index: 2);
            ProjectileManager.Instance.CreateProjectile(controller.transform, controller.attack, attrs: new []{ new StraightAttr()}, degree: -100, index: 2);

        }
    }

    public override void End()
    {
        controller.animationHandler.Set(EnemyAnimationHandler.Run, false);
    }
}
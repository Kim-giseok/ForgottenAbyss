using UnityEngine;

public class MudAggroNode : Node
{
    public override void Start()
    {
        controller.animnHandler.Play("Idle");
    }
    public override void Update()
    {
        if (NavSurface.Instance.GetPlatformId(controller.gameObject) == NavSurface.Instance.GetPlatformId(GameManager.Instance.player.gameObject)) { SetStatus(Status.Success); return;
        }
        controller.LookTarget();
    }
}

public class MudIdleNode : Node
{
    public override void Start()
    {
        controller.animnHandler.Play("Idle");
    }

    public override void Update()
    {
        controller.LookTarget();
        if(currTime > 3f) { SetStatus(Status.Success);}
    }
}

public class MudCastingNode : Node
{
    public override void Start()
    {
        controller.animnHandler.Play("Casting");
    }

    public override void Update()
    {
        controller.LookTarget();
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (status == AnimationStatus.End)
        {
            SetStatus(Status.Success);
        }
    }
}

public class MudAttackNode : Node
{
    public override void Start()
    {
        controller.animnHandler.Play("Attack");
        controller.LookTarget();
        // controller.rigidbody.drag = 4f;
    }
    
    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Attack")) return;

        if (status == AnimationStatus.Start)
        {
            context.Set("currPos", controller.transform.position);

            Collider2D[] nearColiders = Physics2D.OverlapCircleAll(controller.transform.position, 1f, LayerMask.GetMask("Ground"));
            foreach (var nearCollider in nearColiders) { Physics2D.IgnoreCollision(controller.collider, nearCollider, true); }

            BoltsPool.Instance.CreateMelee(controller.transform, 40f).Fire();
            controller.rigidbody.AddForce(controller.agent.GetDirection() * 4f, ForceMode2D.Impulse);
            return;
        }

        if (status == AnimationStatus.End)
        {
            controller.rigidbody.velocity = Vector2.zero;
            controller.transform.position = context.Get<Vector3>("currPos");
            
            Collider2D[] nearColiders = Physics2D.OverlapCircleAll(controller.transform.position, 1f, LayerMask.GetMask("Ground"));
            foreach (var nearCollider in nearColiders) { Physics2D.IgnoreCollision(controller.collider, nearCollider, false); }
            
            SetStatus(Status.Success);
        }
    }

    public override void End()
    {
        BoltsPool.Instance.DisableMelee(controller.transform);
        controller.rigidbody.drag = 0f;
    }
}
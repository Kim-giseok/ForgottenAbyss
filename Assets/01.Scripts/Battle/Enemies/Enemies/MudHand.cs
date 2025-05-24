using UnityEngine;

public class MudSHandpawnNode : Node
{
    public override void Start()
    {
        if (controller is not EnemyController eController || eController.Board.IsSpawned)
        {
            SetStatus(Status.Success);
            return;
        }
        
        for (int currDegree = -180; currDegree <= 90; currDegree += 60)
        {
            {
                BoltsPool.Instance.Create(controller.transform, Bolts.Type.Parabola)
                    .SetEffect(Bolts.EffectType.Penetration)
                    .SetSize(0.6f)
                    .SetDamage(eController.resourceHandler.Get(EnemyStatType.Attack).value)
                    .SetSpeed(8)
                    .SetDegree(currDegree)
                    .SetDuration(0.6f)
                    .Fire();
            }
        }

        controller.Anim.Play("Spawn");
        eController.Board.IsSpawned = true;
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (animInfo.IsName("Spawn") && status == AnimationStatus.End) { SetStatus(Status.Success); }
    }
}

public class MudAggroNode : Node
{
    public override void Start()
    {
        controller.Anim.Play("Idle");
    }
    public override void Update()
    {
        // if (NavSurface.Instance.GetPlatformId(controller.gameObject) == NavSurface.Instance.GetPlatformId(GameManager.Instance.player.gameObject)) { SetStatus(Status.Success); return;
        // }
        // controller.LookTarget();
    }
}

public class MudIdleNode : Node
{
    public override void Start()
    {
        controller.Anim.Play("Idle");
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
        controller.Anim.Play("Casting");
    }

    public override void Update()
    {
        if(controller is EnemyController) { controller.LookTarget(); }
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (status == AnimationStatus.End)
        {
            SetStatus(Status.Success);
        }
    }
}

public class MudHandRandCoolNode : Node
{
    public override void Start()
    {
        context.Set("idleRandomDuration", Random.Range(0.4f, 4f));
        
        controller.Rigid.velocity = new Vector2(0, controller.Rigid.velocity.y);
        
        controller.Anim.Play("Idle");
    }

    public override void Update()
    {
        if (currTime >= context.Get<float>("idleRandomDuration")) { SetStatus(Status.Success); }
    }

}

public class MudAttackNode : Node
{
    public override void Start()
    {
        controller.Anim.Play("Attack");
        if(controller is EnemyController) { controller.LookTarget(); }
        // controller.rigidbody.drag = 4f;
    }
    
    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Attack")) return;

        if (status == AnimationStatus.Start)
        {
            context.Set("currPos", controller.transform.position);

            Collider2D[] nearColiders = Physics2D.OverlapCircleAll(controller.transform.position, 1f, LayerMask.GetMask("Ground"));
            foreach (var nearCollider in nearColiders) { Physics2D.IgnoreCollision(controller.Collider, nearCollider, true); }

            BoltsPool.Instance.CreateMelee(controller.transform, 40f).Fire();

            if(controller is EnemyController) { controller.Rigid.AddForce(controller.Agent.GetDirection() * 4f, ForceMode2D.Impulse); }
            if(controller is SummonController sController) { controller.Rigid.AddForce(Vector2.right * 4f, ForceMode2D.Impulse); }
            
            return;
        }

        if (status == AnimationStatus.End)
        {
            controller.Rigid.velocity = Vector2.zero;
            controller.transform.position = context.Get<Vector3>("currPos");
            
            Collider2D[] nearColiders = Physics2D.OverlapCircleAll(controller.transform.position, 1f, LayerMask.GetMask("Ground"));
            foreach (var nearCollider in nearColiders) { Physics2D.IgnoreCollision(controller.Collider, nearCollider, false); }
            
            SetStatus(Status.Success);
        }
    }

    public override void End()
    {
        BoltsPool.Instance.DisableMelee(controller.transform);
        controller.Rigid.drag = 0f;
    }
}

public class MudHandRangeAttack : Node
{
    public override void Start()
    {
        if (controller is not EnemyController eController)
        {
            SetStatus(Status.Success);
            return;
        }

        for (int currDegree = -180; currDegree <= 90; currDegree += 60)
        {
            {
                BoltsPool.Instance.Create(controller.transform, Bolts.Type.Parabola)
                    .SetEffect(Bolts.EffectType.Penetration)
                    .SetSize(0.6f)
                    .SetDamage(eController.resourceHandler.Get(EnemyStatType.Attack).value)
                    .SetSpeed(8)
                    .SetDegree(currDegree)
                    .SetDuration(3f)
                    .Fire();
            }
        }

        controller.Anim.Play("Attack");
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (animInfo.IsName("Attack") && status == AnimationStatus.End) { SetStatus(Status.Success); }
    }
}
using UnityEngine;
public class SSDashAttack : Node
{
    private string animationName;

    public SSDashAttack(string animationName)
    {
        this.animationName = animationName;
    }

    public override void Start()
    {
        
        controller.animnHandler.Play(animationName);
        controller.LookTarget();

        controller.rigidbody.velocity = Vector2.zero;
        // controller.rigidbody.gravityScale = 0f;
        controller.rigidbody.drag = 4f;
        controller.rigidbody.AddForce(new Vector2(controller.agent.GetDirection().x * 24f, 0), ForceMode2D.Impulse);
    }

    // public override void OnAnimatedEvent(bool isFire)
    // {
        // if(isFire) ProjectileManager.Instance.CreateMeleeProjectile(controller.transform, 10f);
        // else ProjectileManager.Instance.DestroyMeleeProjectile(controller.transform);
    // }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        // notice: 플래그로 관리 필요
        if (!animInfo.IsName("Combo1") && !animInfo.IsName("Combo2") && !animInfo.IsName("Combo3")) return;
        if (status == AnimationStatus.Start)
        {
            BoltsPool.Instance.CreateMelee(controller.transform, 10f);
        }
        
        if (status == AnimationStatus.End)
        {
            BoltsPool.Instance.DestroyMelee(controller.transform);
            SetStatus(Status.Success);
        }
    }

    public override void End()
    {
        controller.rigidbody.drag = 0;
    }
    
}

public class RandomCoolTimeNode : Node
{
    public override void Start()
    {
        context.Set("idleRandomDuration", Random.Range(0.4f, 2f));
        
        controller.rigidbody.velocity = new Vector2(0, controller.rigidbody.velocity.y);
        
        controller.animnHandler.Play("Idle");
    }

    public override void Update()
    {
        if (currTime >= context.Get<float>("idleRandomDuration")) { SetStatus(Status.Success); }
    }

    public override void OnAgentDetected(EnemyAgent.Status status)
    {
        if(status == EnemyAgent.Status.None) { SetStatus(Status.Fail); }
    }
}


public class CoolTimeNode : Node
{
    private float duration;
    
    public CoolTimeNode(float duration) => this.duration = duration;
    public override void Start()
    {
        controller.animnHandler.Play("Idle");
    }

    public override void Update()
    {
        if (currTime >= duration) { SetStatus(Status.Success); }
    }

    public override void OnAgentDetected(EnemyAgent.Status status)
    {
        if(status == EnemyAgent.Status.None) { SetStatus(Status.Fail); }
    }
}
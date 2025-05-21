using UnityEngine;

public class SSCastingNode : Node
{
    public override void Start()
    {
        controller.Anim.Play("Casting");
        controller.LookTarget();
        controller.Rigid.velocity = Vector2.zero;
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Casting")) return;
        if (status == AnimationStatus.End) { SetStatus(Status.Success); }
    }
}

public class SSDashAttack : Node
{
    private string animationName;

    public SSDashAttack(string animationName)
    {
        this.animationName = animationName;
    }

    public override void Start()
    {
        controller.Anim.Play(animationName);
        controller.LookTarget();

        controller.Rigid.velocity = Vector2.zero;
        // controller.rigidbody.gravityScale = 0f;
        controller.Rigid.drag = 4f;
        controller.Rigid.AddForce(new Vector2(controller.Agent.GetDirection().x * 24f, 0), ForceMode2D.Impulse);

        controller.soundHandler.Play(EnemySoundType.Attack);
    }
    
    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        // notice: 플래그로 관리 필요
        if (!animInfo.IsName("Combo1") && !animInfo.IsName("Combo2") && !animInfo.IsName("Combo3")) return;
        if (status == AnimationStatus.Start)
        {
            BoltsPool.Instance.CreateMelee(controller.transform).SetDamage(10).Fire();
        }
        
        if (status == AnimationStatus.End)
        {
            BoltsPool.Instance.DisableMelee(controller.transform);
            SetStatus(Status.Success);
        }
    }

    public override void End()
    {
        controller.Rigid.drag = 0;
    }
    
}

public class RandomCoolTimeNode : Node
{
    public override void Start()
    {
        context.Set("idleRandomDuration", Random.Range(1.4f, 2.8f));
        
        controller.Rigid.velocity = new Vector2(0, controller.Rigid.velocity.y);
        
        controller.Anim.Play("Idle");
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
        controller.Anim.Play("Idle");
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
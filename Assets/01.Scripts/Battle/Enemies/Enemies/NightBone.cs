using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplosionNode : Node
{
    public override void Start()
    {
        controller.animnHandler.Play("Explosion");
    }

    public override void OnAnimatedEvent(bool isFire)
    {
        if (isFire) {
            BoltsPool.Instance.CreateMelee(controller.transform)
                .SetLocalPos(Vector2.zero)
                .SetDamage(20)
                .SetSize(2f)
                .Fire();
            
        }
        else BoltsPool.Instance.DisableMelee(controller.transform);
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Explosion")) return;
        if(status == AnimationStatus.End) { SetStatus(Status.Success); return; }
    }
}

// 공통으로 사용할 수 있을 듯
public class ChargingNode : Node
{
    private float duration = 2;

    public ChargingNode(float duration)
    {
        this.duration = duration;
    }
    
    public override void Start()
    {
        controller.animnHandler.Play("Charging");
    }

    public override void Update()
    {
        controller.LookTarget();
        if(currTime >= duration) { SetStatus(Status.Success); return; }
    }
}

public class StopNode : Node
{
    public override void Start()
    {
        if(controller.agent.status != EnemyAgent.Status.Tracked) { SetStatus(Status.Fail); return; }

        controller.rigidbody.velocity = Vector2.zero;
        SetStatus(Status.Success);
        return;
    }
}

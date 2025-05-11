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

// waitNode에서 casting으로 
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
        controller.soundHandler.Play(EnemySoundType.Casting);
    }

    public override void Update()
    {
        controller.LookTarget();
        if(currTime >= duration) { SetStatus(Status.Success); return; }
    }
}
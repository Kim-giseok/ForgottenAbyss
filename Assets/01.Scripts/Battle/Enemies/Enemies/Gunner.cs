using System;
using UnityEngine;

public class GunnerRangeAttack : Node
{
    public override void Start()
    {
        controller.animnHandler.Play("Attack");
        controller.LookTarget();
    }

    public override void OnAnimatedEvent(bool isFire)
    {
        if (isFire)
        {
            BoltsPool.Instance.Create(controller.transform, Bolts.Type.Laser)
                .SetDirection(controller.agent.GetDirection())
                // .SetTrailCurve(BoltsPool.TrailType.Laser)
                .SetEffect(Bolts.EffectType.Penetration)
                .SetSpeed(20f)
                .Fire();
        }
    }
    
    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Attack")) return;
        if (status == AnimationStatus.End) { SetStatus(Status.Success); }
    }
}
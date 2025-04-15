using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplosionNode : Node
{
    public override void Start()
    {
        controller.animationHandler.Play("Explosion");
    }

    public override void OnAnimatedEvent(bool isFire)
    {
        if (isFire) ProjectileManager.Instance.CreateMeleeProjectile(controller.transform, 10f, Vector2.zero, Vector2.one * 2);
        else ProjectileManager.Instance.DestroyMeleeProjectile(controller.transform);
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
    public override void Start()
    {
        controller.animationHandler.Play("Charging");
    }

    public override void Update()
    {
        if(currTime >= duration) { SetStatus(Status.Success); return; }
    }
}

public class StopNode : Node
{
    public override void Start()
    {
        controller.rigidbody.velocity = Vector2.zero;
        SetStatus(Status.Success);
        return;
    }
}

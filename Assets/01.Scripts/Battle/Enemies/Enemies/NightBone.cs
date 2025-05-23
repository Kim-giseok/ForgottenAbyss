using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplosionNode : Node
{
    public override void Start()
    {
        controller.Anim.Play("Explosion");
    }

    public override void OnAnimatedEvent(bool isFire)
    {
        if (isFire) {
            BoltsPool.Instance.CreateMelee(controller.transform)
                .SetLocalPos(Vector2.zero)
                .SetDamage(((EnemyController)controller).resourceHandler.Get(EnemyStatType.Attack).value * 2)
                .SetSize(2f)
                .Fire();
            
            BoltsPool.Instance.Particle(controller.transform, "NightBone_Explosion")
                .SetSize(2.5f).SetPosition(controller.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.6f + Random.Range(-0.2f, 0.2f))).Play();

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
        controller.Anim.Play("Charging");
        SoundManager.Instance.Playsfx("ElectronicCast");
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
        controller.Rigid.velocity = Vector2.zero;
        SetStatus(Status.Success);
        return;
    }
}
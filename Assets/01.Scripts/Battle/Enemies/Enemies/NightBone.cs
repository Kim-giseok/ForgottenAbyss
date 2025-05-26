using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// notice: 애니메이션의 이름만 다르고, 공격 형태는 유사하다.
public class ExplosionNode : Node
{
    public override void Start()
    {
        controller.Anim.Play("Explosion");
    }

    public override void OnAnimatedEvent(bool isFire)
    {
        if (isFire) {
            // 스킬인 경우, 계수 개념 등록하기
            BoltsPool.Instance.CreateMelee(controller.transform).SetLocalPos(Vector2.zero).SetDamage(((EnemyController)controller).Resource.Get(EnemyStatType.Attack).value * 2).SetSize(2f).Fire();
            BoltsPool.Instance.Particle(controller.transform, "NightBone_Explosion").SetSize(2.5f).SetPosition(controller.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.6f + Random.Range(-0.2f, 0.2f))).Play();

        }
        else BoltsPool.Instance.DisableMelee(controller.transform);
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if(status == AnimationStatus.End) { SetStatus(Status.Success); }
    }
}

// 공통으로 사용할 수 있을 듯 - WaitNode 로
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
        if(currTime >= duration) { SetStatus(Status.Success); }
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
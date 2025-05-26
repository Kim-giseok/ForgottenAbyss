using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


// 무기가 Node를 결정할 수 있도록
public class MeleeAttack : Node
{
    public override void Start()
    {
        controller.LookTarget();
        controller.Anim.Play("Attack");
    }

    public override void OnAnimatedEvent(bool isFire)
    {
        if (isFire)
        {
            controller.soundHandler.Play(EnemySoundType.Attack);
            // 데미지나 사이즈등은 추상화로 접급
            BoltsPool.Instance.CreateMelee(controller.transform, ((EnemyController)controller).Resource.Get(EnemyStatType.Attack).value).Fire();
            // NightBone만 실행되어야하여 전략패턴으로 빼야함
            BoltsPool.Instance.Particle(controller.transform, "Slash").SetSize(2.5f).SetPosition(controller.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.6f + Random.Range(-0.2f, 0.2f))).Play();
        }
        else
        {
            BoltsPool.Instance.DisableMelee(controller.transform);
        }
    }

    // fix: 의미 없는 호출이 발생할 수 있느 점에 대한 고려 필요
    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (status == AnimationStatus.End) { SetStatus(Status.Success); }
    }
    
    public override void End() // notice: 공격 중 피격 당하는 경우
    {
        BoltsPool.Instance.DisableMelee(controller.transform);
    }
}

public class RangeAttackNode : Node
{
    public override void Start()
    {
        controller.Anim.Play("Attack");
        controller.LookTarget();
    }
    
    public override void OnAnimatedEvent(bool isFire)
    {
        // 콜백으로 공격 방식 추상화
        if (isFire)
        {
            BoltsPool.Instance.Create(controller.transform, Bolts.Type.Linear).SetDamage(10).SetDirection(controller.Agent.GetDirection()).Fire();
        }
    }
    
    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Attack")) return;
        if (status == AnimationStatus.End) { SetStatus(Status.Success); }
    }
}
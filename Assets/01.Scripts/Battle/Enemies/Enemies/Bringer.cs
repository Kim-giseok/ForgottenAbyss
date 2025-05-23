using UnityEngine;

public class BringerAttackNode : Node
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
            // 데미지나 사이즈등은 추상화로 접급
            BoltsPool.Instance.CreateMelee(controller.transform)
                .SetDamage(((EnemyController)controller).resourceHandler.Get(EnemyStatType.Attack).value)
                .SetKnockBack(20)
                .SetSize(3f, 2f)
                .Fire();
        }
        else
        {
            BoltsPool.Instance.DisableMelee(controller.transform);
        }
    }

    // fix: 의미 없는 호출이 발생할 수 있느 점에 대한 고려 필요
    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Attack")) return;
        if (status == AnimationStatus.End) { SetStatus(Status.Success); }
    }
    
    public override void End() // notice: 공격 중 피격 당하는 경우
    {
        BoltsPool.Instance.DisableMelee(controller.transform);
    }
}

// 무기가 Node를 결정할 수 있도록

using UnityEngine;

// 기본 노드로 빼내기
public class MeleeAttack : Node
{
    public override void Start()
    {
        controller.LookTarget();
        controller.animnHandler.Play("Attack");
    }

    public override void OnAnimatedEvent(bool isFire)
    {
        if (isFire)
        {
            SoundManager.Instance.PlaySFX(controller.soundHandler.GetClip(EnemySoundType.Attack));
            // 데미지나 사이즈등은 추상화로 접급
            BoltsPool.Instance.CreateMelee(controller.transform, controller.combatHandler.power).Fire();
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
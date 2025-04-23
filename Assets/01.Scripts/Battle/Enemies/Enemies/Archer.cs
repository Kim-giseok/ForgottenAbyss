using UnityEngine;

public class RangeMultiAttackNode : Node
{
    public override void Start()
    {
        controller.animnHandler.Play("Attack");
        controller.LookTarget();
    }
    
    public override void OnAnimatedEvent(bool isFire)
    {
        // 콜백으로 공격 방식 추상화
        if (isFire)
        {
            for (int currDegree = -20; currDegree <= 20; currDegree += 10)
            {
                BoltsPool.Instance.Create(controller.transform, Bolts.Type.Decrescendo)
                    .SetSprite("arrow")
                    .SetSize(1f)
                    .SetDamage(10)
                    .SetDegree(controller.agent.GetDegree() + currDegree)
                    .SetDuration(0.6f)
                    .Fire();
            }
        }
    }
    
    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Attack")) return;
        if (status == AnimationStatus.End) { SetStatus(Status.Success); }
    }
}

public class PlayRollingAnimation : Node
{
    public override void Start()
    {
        controller.animnHandler.Play("Rolling");
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if(animInfo.IsName("Rolling") && status == AnimationStatus.End) { SetStatus(Status.Success); }
    }
}
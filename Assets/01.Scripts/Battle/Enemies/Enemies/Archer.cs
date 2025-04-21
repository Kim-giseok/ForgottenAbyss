// Attack은 추상화할 수 있는 편

using UnityEngine;

public class RangeMultiAttackNode : Node
{
    private readonly int index;

    public RangeMultiAttackNode(int index)
    {
        this.index = index;
    }
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
                BoltManager.Instance.CreateProjectile(controller.transform, 10, index: index, degree: BoltManager.GetDegreeByDirection(controller.agent.GetDirection()) + currDegree);
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
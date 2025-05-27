using UnityEngine;


public class RangeMultiAttackNode : Node
{
    public override void Start()
    {
        controller.Anim.Play("Attack");
        // LookTarget을 조건에 따라 다르게 설정하기
        if (controller is EnemyController) { controller.LookTarget(); }
    }
    
    public override void OnAnimatedEvent(bool isFire)
    {
        // 콜백으로 공격 방식 추상화
        if (isFire)
        {
            for (int currDegree = -10; currDegree <= 10; currDegree += 10)
            {
                if (controller is EnemyController eController)
                {
                    BoltsPool.Instance.Create(controller.transform, Bolts.Type.Decrescendo)
                        .SetSprite("arrow")
                        .SetEffect(Bolts.EffectType.Penetration)
                        .SetSize(1f)
                        .SetDamage(eController.Resource.Get(EnemyStatType.Attack).value)
                        .SetSpeed(35)
                        .SetDegree(controller.Agent.GetDegree() + currDegree)
                        .SetDuration(0.8f)
                        .Fire();
                }
            }
            
            if (controller is SummonController sController)
            {
                for (var currDegree = 0; currDegree <= 360; currDegree += 30)
                {
                    BoltsPool.Instance.Create(controller.transform, Bolts.Type.Decrescendo)
                        .SetSprite("arrow")
                        .SetSize(1f)
                        .SetDamage(4)
                        .SetKnockBack(4)
                        .SetEffect(Bolts.EffectType.Penetration)
                        .SetSpeed(60)
                        .SetDegree(currDegree)
                        .SetDuration(0.4f)
                        .Fire();
                }
            }
        }
    }
    
    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (status == AnimationStatus.End) { SetStatus(Status.Success); }
    }
}

// 회피 관련, dash-move
public class PlayRollingAnimation : Node
{
    public override void Start()
    {
        controller.Anim.Play("Rolling");
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if(status == AnimationStatus.End) { SetStatus(Status.Success); }
    }
}

public class LookTargetNode : Node
{
    public override void Start()
    {
        controller.Rigid.velocity = Vector3.zero;
        controller.Anim.Play("Idle");
    }

    public override void Update()
    {
        controller.LookTarget();
    }
}
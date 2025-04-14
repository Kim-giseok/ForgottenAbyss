using UnityEngine;


// 무기가 Node를 결정할 수 있도록
public class MeleeAttack : Node
{
    public override void Start()
    {
        controller.animationHandler.Set(EnemyAnimationHandler.Attack);
        controller.LookTarget();
    }

    public override void OnAnimatedEvent(bool isFire)
    {
        if (isFire)
        {
            // 데미지나 사이즈등은 추상화로 접급
            ProjectileManager.Instance.CreateMeleeProjectile(controller.transform, controller.combatHandler.power);
        }
        else
        {
            ProjectileManager.Instance.DestroyMeleeProjectile(controller.transform);
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
        ProjectileManager.Instance.DestroyMeleeProjectile(controller.transform);
    }
}

// Attack은 추상화할 수 있는 편
public class RangeAttackNode : Node
{
    public override void Start()
    {
        controller.animationHandler.Set(EnemyAnimationHandler.Attack);
        controller.LookTarget();
    }

    public override void OnAnimatedEvent(bool isFire)
    {
        // 콜백으로 공격 방식 추상화
        if (isFire)
        {
            ProjectileManager.Instance.CreateProjectile(controller.transform, 3, degree: ProjectileManager.Instance.GetDegreeByDirection(controller.agent.GetDirection()));
        }
    }
    
    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Attack")) return;
        if (status == AnimationStatus.End) { SetStatus(Status.Success); }
    }
}

// 초기에 방향을 결정해서 쏜다는 점 만 추가됨
public class RangeToTargetNode : Node
{
    public override void Start()
    {
        controller.animationHandler.Set(EnemyAnimationHandler.Attack); // 공통 부분
    }

    public override void OnAnimatedEvent(bool isFire)
    {
        if (isFire)
        {
        }
    }


    public override void End() // 애니메이션 끝나지 않았는데 공격 당하는 경우( 공격 노드에서 처리)
    {
    }
}

// 동시 다발적으로 발사하는 경우 - 이 부분도 일반 공격으로부터 파생 가능
public class ParallelShotNode : Node
{
    private Vector2 direction;
}
using System.Collections;
using UnityEngine;

// controller에게 부탁하여 코루틴을 실행할 순 있다.
public class HealNode : Node
{
    public override void Start()
    {
        controller.animHandler.Play("Heal");
        Collider2D[] hits = Physics2D.OverlapCircleAll(controller.transform.position, 200f, LayerMask.GetMask("Enemy"));
        foreach (var hit in hits)
        {
            if (hit.gameObject == controller.gameObject) continue;
            if (!hit.TryGetComponent(out EnemyController econtoller)) continue;
            BoltsPool.Instance.Create(hit.transform, Bolts.Type.Heal).SetSize(1f).Fire();
            if(econtoller.health <= 30) econtoller.health += 10;
        }
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Heal")) return;
        if(status == AnimationStatus.End) { SetStatus(Status.Success); return; }
    }
}


public class WizadRecursiveNode : Node
{
    public override void Start()
    {
        controller.animHandler.Play("Attack");
        controller.LookTarget();
    }
    
    public override void OnAnimatedEvent(bool isFire)
    {
        // 콜백으로 공격 방식 추상화
        if (isFire)
        {
            BoltsPool.Instance.Create(controller.transform, Bolts.Type.Recursive)
                .SetSize(0.6f).SetDamage(10).SetSpeed(4).SetDegree(controller.agent.GetDegree())
                .SetEffect(Bolts.EffectType.Penetration).SetDuration(1.6f).Fire();
        }
    }
    
    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Attack")) return;
        if (status == AnimationStatus.End) { SetStatus(Status.Success); }
    }
}
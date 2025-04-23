using System.Collections;
using UnityEngine;

// controller에게 부탁하여 코루틴을 실행할 순 있다.
public class HealNode : Node
{
    public override void Start()
    {
        controller.animnHandler.Play("Heal");
        Collider2D[] hits = Physics2D.OverlapCircleAll(controller.transform.position, 200f, LayerMask.GetMask("Enemy"));
        foreach (var hit in hits)
        {
            if (hit.gameObject == controller.gameObject) continue;
            if (!hit.TryGetComponent(out EnemyController econtoller)) continue;
            BoltsPool.Instance.Create(hit.transform, Bolts.Type.Heal);
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
        controller.animnHandler.Play("Attack");
        controller.LookTarget();
    }
    
    public override void OnAnimatedEvent(bool isFire)
    {
        // 콜백으로 공격 방식 추상화
        if (isFire)
        {
            BoltsPool.Instance.Create(controller.transform, Bolts.Type.Recursive)
            .SetDamage(10)
            .SetDegree(controller.agent.GetDegree())
            .SetDuration(1f)
            .Fire();
        }
    }
    
    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Attack")) return;
        if (status == AnimationStatus.End) { SetStatus(Status.Success); }
    }
}
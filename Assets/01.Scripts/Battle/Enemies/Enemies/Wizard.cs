using System.Collections;
using UnityEngine;

// controller에게 부탁하여 코루틴을 실행할 순 있다.
public class HealNode : Node
{
    public override void Start()
    {
        controller.animationHandler.Play("Heal");
        Collider2D[] hits = Physics2D.OverlapCircleAll(controller.transform.position, 200f, LayerMask.GetMask("Enemy"));
        foreach (var hit in hits)
        {
            if (hit.gameObject == controller.gameObject) continue;
            if (!hit.TryGetComponent(out EnemyController econtoller)) continue;
            ProjectileManager.Instance.CreateProjectile(hit.transform, 0, index: 2);
            if(econtoller.health <= 30) econtoller.health += 10;
        }
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Heal")) return;
        if(status == AnimationStatus.End) { SetStatus(Status.Success); return; }
    }
}
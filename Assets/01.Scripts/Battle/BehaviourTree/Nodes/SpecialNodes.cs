using UnityEngine;

public class GuardNode : Node
{
    public override void Start()
    {
        if (controller is EnemyController enemyController)
        {
            enemyController.statusHandler.isDefense = true;
        }
    }

    public override void End()
    {
        // controller.statusHandler.isDefense = false;
    }
}

// knockBack이 들어갈 수도 있도록
// 피격 애니메이션 자체는 발생하더라도 바로 액션 끝나도록
public class HitNode : Node
{
    private bool isNockBack = false; // 내부 변수 불가능
    
    public override void Start()
    {
        // do: 시퀀스를 통행 애초에 진입이 안되도록 노드 지정
        if (controller is not EnemyController eController) return;
        if (!eController.statusHandler.isHit) { SetStatus(Status.Fail); return; }
        if(eController.health <= 0) { SetStatus(Status.Success); return; }
     
        // 타격 받은 쪽으로 회전
        // Vector2 direction = (controller.agent.player.transform.position - controller.transform.position).normalized;
        // controller.Flip(direction.x > 0);
        
        if(eController.statusHandler.isIgnoreHitAction) eController.spriteRenderer.color = Color.red;
        eController.animationHandler.Play("Hit");
        eController.statusHandler.isHit = false;

        // 애니메이션이 바로 바뀌어 꺼지는 현상과 충돌
        if (eController.statusHandler.isIgnoreHitAction && currTime >= 0.2f) { SetStatus(Status.Fail); }
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Hit") || status != AnimationStatus.End) return;
        SetStatus(Status.Fail);
    }
}

public class DieNode : Node
{
    public override void Start()
    {
        if (controller is not EnemyController eController || eController.health > 0) { SetStatus(Status.Fail); return;}
        
        controller.collider.enabled = false; // 죽은 이후로는 피격 불가능하도록 처리
        controller.rigidbody.gravityScale = 0; // 바닥 아래로 떨어지는 현상 방지
        eController.animationHandler.Play("Die");
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Die") || controller is not EnemyController eController || status != AnimationStatus.End) return;
        eController.Die();
    }
}
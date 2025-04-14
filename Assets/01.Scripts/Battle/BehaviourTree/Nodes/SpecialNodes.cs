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
public class HitNode : Node
{
    private bool isNockBack = false; // 내부 변수 불가능
    
    public override void Start()
    {
        if (controller is not EnemyController eController) return;
        if (!eController.statusHandler.isHit) // do: 시퀀스를 통행 애초에 진입이 안되도록 노드 지정
        {
            SetStatus(Status.Fail);
            return;
        }
        
        // 타격 받은 쪽으로 회전
        // Vector2 direction = (controller.agent.player.transform.position - controller.transform.position).normalized;
        // controller.Flip(direction.x > 0);
        
        if(eController.statusHandler.isIgnoreHitAction) eController.spriteRenderer.color = Color.red;
        controller.animationHandler.Play("Hit");

        eController.statusHandler.isHit = false;
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Hit")) return;
        if (status == AnimationStatus.End)
        {
            SetStatus(Status.Success);
        }
    }
}

public class DieNode : Node
{
    public override void Start()
    {
        if (controller is EnemyController enemyController)
        {
            enemyController.Die();
        }
    }
}
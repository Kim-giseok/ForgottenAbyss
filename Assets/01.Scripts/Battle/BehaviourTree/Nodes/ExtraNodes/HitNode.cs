using System.Collections;
using UnityEngine;

// knockBack이 들어갈 수도 있도록
// 피격 애니메이션 자체는 발생하더라도 바로 액션 끝나도록

public class HitNode : Node
{
    IEnumerator BlinkRed()
    {
        // 디졸브 shader인 경우에만 가능
        controller.Renderer.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);

        controller.Renderer.material.color = Color.white;
        yield return new WaitForSeconds(0.2f);
    }
    
    public override void Start()
    {

        // do: 시퀀스를 통행 애초에 진입이 안되도록 노드 지정
        if (controller is not EnemyController eController) return;
        if (!eController.statusHandler.isHit) { SetStatus(Status.Fail); return; }
        if(eController.health <= 0) { SetStatus(Status.Success); return; }

        // 타격 받은 쪽으로 회전
        // Vector2 direction = (controller.agent.player.transform.position - controller.transform.position).normalized;
        // controller.Flip(direction.x > 0);
        
        eController.statusHandler.isHit = false;

        controller.soundHandler.Play(EnemySoundType.Hit);
        if (eController.isIgnoreHitAnim)
        {
            controller.StartCoroutine(BlinkRed());
            SetStatus(Status.Fail);
            return;
        }
        
        eController.animnHandler.Play("Hit");
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Hit") || status != AnimationStatus.End) return;
        SetStatus(Status.Fail);
    }
}
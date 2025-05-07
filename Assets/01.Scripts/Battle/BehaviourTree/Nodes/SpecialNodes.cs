using System;
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
    public override void Start()
    {

        // do: 시퀀스를 통행 애초에 진입이 안되도록 노드 지정
        if (controller is not EnemyController eController) return;
        if (!eController.statusHandler.isHit) { SetStatus(Status.Fail); return; }
        if(eController.health <= 0) { SetStatus(Status.Success); return; }

        // 타격 받은 쪽으로 회전
        // Vector2 direction = (controller.agent.player.transform.position - controller.transform.position).normalized;
        // controller.Flip(direction.x > 0);
        
        if(eController.statusHandler.isIgnoreHitAction) eController.Renderer.color = Color.red;
        
        // 순서 바뀌면 문제 생길 수 있음
        var currSoundClip = controller.soundHandler.GetClip(EnemySoundType.Hit);
        if(currSoundClip) SoundManager.Instance.PlaySFX(currSoundClip);

        eController.animnHandler.Play("Hit");
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
        
        controller.Collider.enabled = false; // 죽은 이후로는 피격 불가능하도록 처리
        controller.Rigidbody.velocity = Vector3.zero; // fix: 넉백으로 날라가는 현상 발생
        controller.Rigidbody.isKinematic = true;

        // SoundManager.Instance.PlaySFX(controller.soundHandler.GetClip(EnemySoundHandler.SoundType.Hit));
        
        eController.animnHandler.Play("Die");
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Die") || controller is not EnemyController eController || status != AnimationStatus.End) return;
        eController.Die();
    }
}
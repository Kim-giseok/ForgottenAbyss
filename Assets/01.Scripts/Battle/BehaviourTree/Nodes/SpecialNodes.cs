using System;
using System.Collections;
using UnityEngine;

public class GuardNode : Node
{
    public override void Start()
    {
        if (controller is EnemyController enemyController)
        {
            // enemyController.statusHandler.isDefense = true;
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
        // do: 조건 노드로 빼기
        if (!eController.statusHandler.GetMode(EnmeyMode.Hit)) { SetStatus(Status.Fail); return; }
        if(eController.resourceHandler.Get(EnemyStatType.Health).value <= 0) { SetStatus(Status.Success); return; }
        
        eController.statusHandler.SetMode(EnmeyMode.Hit, false);
        eController.Anim.Play("Hit");
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
        if (controller is not EnemyController eController || eController.resourceHandler.Get(EnemyStatType.Health).value > 0) { SetStatus(Status.Fail); return;}
        
        if (eController.statusHandler.GetMode(EnmeyMode.Die)) { SetStatus(Status.Fail); return; }
        eController.statusHandler.SetMode(EnmeyMode.Die, true);
        
        machine.isIgnoreNotify = true;

        controller.Collider.enabled = false; // 죽은 이후로는 피격 불가능하도록 처리
        controller.Rigid.velocity = Vector3.zero; // fix: 넉백으로 날라가는 현상 발생
        controller.Rigid.isKinematic = true;

        controller.Anim.Play("Die");
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Die") || controller is not EnemyController eController || status != AnimationStatus.End) return; 
        
        eController.Die();
        machine.isIgnoreNotify = false;
    }
}
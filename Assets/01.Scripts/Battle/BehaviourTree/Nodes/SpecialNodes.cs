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
// 데미지 갱신이 내부에서 발생하지 않으면서 순수 노드 역할이 커짐
public class HitNode : Node<EnemyController>
{
    public override void Start()
    {
        controller.Status.SetMode(EnmeyMode.Hit, false);
        controller.Anim.Play("Hit");
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (status == AnimationStatus.End) {SetStatus(Status.Fail);}
    }
}

public class DieNode : Node<EnemyController>
{
    public override void Start()
    {
        
        machine.isIgnoreNotify = true;

        controller.Collider.enabled = false; // 죽은 이후로는 피격 불가능하도록 처리
        controller.Rigid.velocity = Vector3.zero; // fix: 넉백으로 날라가는 현상 발생
        controller.Rigid.isKinematic = true;

        controller.Anim.Play("Die");
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (status != AnimationStatus.End) return; 
        
        controller.Die();
        machine.isIgnoreNotify = false;
    }
}
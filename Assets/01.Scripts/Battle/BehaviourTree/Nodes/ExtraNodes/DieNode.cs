using UnityEngine;

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
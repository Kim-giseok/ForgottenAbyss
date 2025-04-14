using UnityEngine;

namespace Summon
{
    public class DashAttack : Node
    {
        public override void Start()
        {
            if (controller is not SummonController sController) { SetStatus(Status.Fail); return; }
            
            controller.animationHandler.Set(EnemyAnimationHandler.Attack);
            context.Set("direction", sController.tRigidbody.velocity.normalized);
            
            sController.Flip(Mathf.Approximately(sController.target.eulerAngles.y, 0));
            
            controller.rigidbody.velocity = sController.tRigidbody.velocity.normalized * 30f;
            controller.rigidbody.drag = 10f;

        }

        public override void OnAnimatedEvent(bool isFire)
        {
            if (isFire)
            {
                // notice: 공격력은 어케 넣어줘야 할까?
                ProjectileManager.Instance.CreateMeleeProjectile(controller.transform, 10f);
            }
        }

        public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
        {
            if (!animInfo.IsName("Attack")) return;
            if (status == AnimationStatus.End)
            {
                controller.rigidbody.drag = 0;
                SetStatus(Status.Success);
            }
        }

        public override void End()
        {
            ProjectileManager.Instance.DestroyMeleeProjectile(controller.transform);

        }
    }
    
    public class ExplosionSelf: Node {}
}

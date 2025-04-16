using UnityEngine;

namespace Summon
{
    public class CancelAttached : Node
    {
        public override void Start()
        {
            if (controller is not SummonController sController) { SetStatus(Status.Fail); return; }
            sController.CancelAttached();
            SetStatus(Status.Success);
        }
    }
    
    public class DashAttack : Node
    {
        public override void Start()
        {
            if (controller is not SummonController sController) { SetStatus(Status.Fail); return; }
            
            controller.animationHandler.SetSpeed(2f);
            controller.animationHandler.Play("Attack");
            
            context.Set("direction", sController.cRigidbody.velocity.normalized);
            sController.Flip(Mathf.Approximately(sController.caster.eulerAngles.y, 0));
            
            controller.rigidbody.velocity = sController.cRigidbody.velocity.normalized * 80f;
            controller.rigidbody.drag = 20f;

        }

        public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
        {
            if (!animInfo.IsName("Attack")) return;
            
            if (status == AnimationStatus.Start)
            {
                ProjectileManager.Instance.CreateMeleeProjectile(controller.transform, 10f, Vector2.zero, Vector2.one * 2);
                return;
            }
            
            if (status == AnimationStatus.End)
            {
                controller.rigidbody.drag = 0;
                SetStatus(Status.Success);
            }
        }

        public override void End()
        {
            controller.animationHandler.SetSpeed(1f);
            ProjectileManager.Instance.DestroyMeleeProjectile(controller.transform);
        }
    }

    public class Explosion : Node
    {
        public override void Start()
        {
            if(controller is not SummonController sController) { SetStatus(Status.Fail); return; }
            sController.animationHandler.Play("Explosion");
            controller.animationHandler.SetSpeed(1f);
        }

        public override void OnAnimatedEvent(bool isFire)
        {
            if (isFire) ProjectileManager.Instance.CreateMeleeProjectile(controller.transform, 10f, Vector2.zero, Vector2.one * 2);
            else ProjectileManager.Instance.DestroyMeleeProjectile(controller.transform);
        }

        public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
        {
            if (!animInfo.IsName("Explosion")) return;
            if(status == AnimationStatus.End) { SetStatus(Status.Success); return; }
        }

        public override void End()
        {
            controller.animationHandler.SetSpeed(1f);
        }
    }
}

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
            
            controller.animnHandler.SetSpeed(2f);
            controller.animnHandler.Play("Attack");
            
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
                BoltManager.Instance.CreateMelee(controller.transform, 10f, Vector2.zero, Vector2.one * 2);
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
            controller.animnHandler.SetSpeed(1f);
            BoltManager.Instance.DestroyMelee(controller.transform);
        }
    }

    public class Explosion : Node
    {
        public override void Start()
        {
            if(controller is not SummonController sController) { SetStatus(Status.Fail); return; }
            sController.animnHandler.Play("Explosion");
            controller.animnHandler.SetSpeed(1f);
        }

        public override void OnAnimatedEvent(bool isFire)
        {
            if (isFire) BoltManager.Instance.CreateMelee(controller.transform, 10f, Vector2.zero, Vector2.one * 2);
            else BoltManager.Instance.DestroyMelee(controller.transform);
        }

        public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
        {
            if (!animInfo.IsName("Explosion")) return;
            if(status == AnimationStatus.End) { SetStatus(Status.Success); return; }
        }

        public override void End()
        {
            controller.animnHandler.SetSpeed(1f);
        }
    }
}

using UnityEngine;

public class ComboAttackNode : Node
{
    public override void Start()
    {
        context.Set("ssMaxComboCount", Random.Range(1, 3));
        context.Set("ssComboCount", 1);
        controller.animationHandler.Play("Combo1");
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Combo1") && !animInfo.IsName("Combo2") && !animInfo.IsName("Combo3")) return;
        if (status == AnimationStatus.End)
        {
            int currComboCount = context.Get<int>("ssComboCount");
            Debug.Log(currComboCount);
            
            if (currComboCount > context.Get<int>("ssMaxComboCount")) { SetStatus(Status.Success); return; }
            
            context.Set("ssComboCount", currComboCount + 1);
            controller.animationHandler.Play($"Combo{currComboCount}");
        }
    }
}
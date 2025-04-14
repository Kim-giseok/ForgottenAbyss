using System.Collections.Generic;
using UnityEngine;

public class AnimationBehaviour: StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.TryGetComponent(out EnemyBaseController controller)) return;
        
        controller.machine.currNode.SetController(controller);
        controller.machine.currNode.OnAnimated(Node.AnimationStatus.Start, stateInfo);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.TryGetComponent(out EnemyBaseController controller)) return;
        
        controller.machine.currNode.SetController(controller);
        controller.machine.currNode.OnAnimated(Node.AnimationStatus.End, stateInfo);
    }
}
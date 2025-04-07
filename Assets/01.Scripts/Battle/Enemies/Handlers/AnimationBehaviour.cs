using UnityEngine;

public class AnimationBehaviour: StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.TryGetComponent(out EnemyController controller)) return;
        controller.btMachine.currNode.OnAnimated(Node.AnimationStatus.Start, animator);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.TryGetComponent(out EnemyController controller)) return;
        controller.btMachine.currNode.OnAnimated(Node.AnimationStatus.End, animator);
    }
}
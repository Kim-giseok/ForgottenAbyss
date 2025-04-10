using UnityEngine;

public class AnimationBehaviour: StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.TryGetComponent(out EnemyController controller)) return;
        controller.btMachine.currNode.OnAnimated(Node.AnimationStatus.Start, stateInfo);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 애니메이션이 먼저 실행되는 현상 없어야 함, 현재 노드에서 시작이 우선 진행되었는 지 체크하기
        if (!animator.TryGetComponent(out EnemyController controller)) return;
        controller.btMachine.currNode.OnAnimated(Node.AnimationStatus.End, stateInfo);
    }
}
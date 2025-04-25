using UnityEngine;

public class EnemyAnimHandler: MonoBehaviour
{
    private EnemyBaseController controller;
    private Animator animator;
    
    public enum Status { None, Start, End } 
    private Status currStatus = Status.None;
    private int currClipHash;
    
    public void SetController(RuntimeAnimatorController newController) => animator.runtimeAnimatorController = newController;
    // fix: 다시 시작 시 재생되지 않는 현상 수정
    public void Play(string animationName) => animator.Play(animationName, 0, 0f);
    public void SetSpeed(float speed) => animator.speed = speed;
    public void Refresh() => currStatus = Status.None;
    
    private void Awake()
    {
        controller = GetComponent<EnemyBaseController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float progress = stateInfo.normalizedTime % 1f;

        if (stateInfo is { normalizedTime: >= 1f, loop: false }) return;

        // 애니메이션이 변경되면 상태 처음부터 다시 시작
        if (currClipHash != stateInfo.shortNameHash)
        {
            currClipHash = stateInfo.shortNameHash;
            currStatus = Status.None;
        }
        
        if (progress < 0.05f && currStatus != Status.Start)
        {
            currStatus = Status.Start;
            
            if (controller.machine.currNode == null) return;
            controller.machine.currNode.SetController(controller);
            controller.machine.currNode.OnAnimated(Node.AnimationStatus.Start, stateInfo);
        }
        
        if (progress > 0.95f && currStatus != Status.End)
        {
            currStatus = Status.End;

            if (controller.machine.currNode == null) return;
            
            // 에러 발생 확인 필요
            controller.machine.currNode.SetController(controller);
            controller.machine.currNode.OnAnimated(Node.AnimationStatus.End, stateInfo);
        }   
    }
}
using System;
using UnityEngine;

public class EnemyAnimationHandler: MonoBehaviour
{
    private EnemyBaseController controller;
    private Animator animator;
    
    public enum Status { Start, End } 
    private Status currStatus;

    public void SetController(RuntimeAnimatorController newController)
    {
        animator.runtimeAnimatorController = newController;
    }

    public void Play(string animationName)
    {
        animator.Play(animationName);
    }
    
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

        // 시작 부분인지 확인 (예: 5% 이내)
        if (progress < 0.05f && currStatus != Status.Start)
        {
            currStatus = Status.Start;
            
            controller.machine.currNode.SetController(controller);
            controller.machine.currNode.OnAnimated(Node.AnimationStatus.Start, stateInfo);
        }

        // 끝 부분인지 확인 (예: 마지막 5%)
        if (progress > 0.95f && currStatus != Status.End)
        {
            currStatus = Status.End;
            
            controller.machine.currNode.SetController(controller);
            controller.machine.currNode.OnAnimated(Node.AnimationStatus.End, stateInfo);
        }   
    }
}
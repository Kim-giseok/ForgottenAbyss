using System;
using UnityEngine;

public class EnemyAnimHandler: MonoBehaviour
{
    private EnemyBaseController controller;
    public Animator animator { get; private set; }

    private enum Status { None, Start, End } 
    private Status currStatus = Status.None;
    private int currClipHash;
    
    public void SetController(RuntimeAnimatorController newController) => animator.runtimeAnimatorController = newController;
    // fix: 다시 시작 시 재생되지 않는 현상 수정
    public void Play(string animationName)
    {
        animator.Play(animationName, 0, 0f);
        currStatus = Status.None;
    }

    public void SetSpeed(float speed) => animator.speed = speed;
    
    private void Awake()
    {
        controller = GetComponent<EnemyBaseController>();
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (currClipHash != stateInfo.shortNameHash)
        {
            currClipHash = stateInfo.shortNameHash;
            currStatus = Status.Start;

            controller.Machine.currNode?.SetController(controller);
            controller.Machine.currNode?.OnAnimated(Node.AnimationStatus.Start, stateInfo);
        }

        // 애니메이션 종료 감지 (1바퀴 이상 돌았을 때) - start가 무조건 먼저 발생하기 때문에 기존 end가 먼저 실행되는 문제 발생 X
        if (stateInfo.normalizedTime >= 1f && currStatus == Status.Start)
        {
            currStatus = Status.End;

            controller.Machine.currNode?.SetController(controller);
            controller.Machine.currNode?.OnAnimated(Node.AnimationStatus.End, stateInfo);
        }
    }
}
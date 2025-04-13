using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    protected Node parent;
    public List<Node> children { get; private set; } = new();
    
    protected EnemyBaseController controller; 
    protected BTMachine machine;
    protected BTContext context;
    
    public float currTime => machine.currTime;
    
    public enum Status { Success, Fail }
    public enum AnimationStatus { Start, End }

    public void SetController(EnemyBaseController controller)
    {
        this.controller = controller;
        machine = controller.machine;
        context = machine.context;
    }

    public void SetParent(Node parent)
    {
        this.parent = parent;
    }

    public void SetStatus(Status newStatus)
    {
        machine.SetPlaying(false);
        
        parent.SetController(controller); // 여기서도 전달 불가 발생
        parent.GetStatus(newStatus, this);
    }

    // 모든 이벤트가 직전에 node의 controller 변경해주는 작업이 발생함
    public virtual void Start() {}
    public virtual void Update() {}
    public virtual void End() {}

    public virtual void OnAnimatedEvent(bool isFire) {}
    public virtual void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo) {}
    
    public virtual void GetStatus(Status newStatus, Node caller) {}
    
    // notice : EnemyDetectHandler 에서 앞으로 갈 수 있는 지 등의 정보를 전달
    public virtual void OnDetected(EnemyDetectHandler.DetectType detectType, bool value) {}
    
    // notice: 블랙보드에게 특정 정보를 받는 경우
    public virtual void OnStatusChanged() {}
    
    // 타겟 감지 등의 정보
    public virtual void OnAgentDetected() {}
}
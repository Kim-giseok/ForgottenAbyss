using UnityEngine;

public abstract class Node
{
    protected Node parent;
    protected EnemyController controller; 
    protected BTMachine btMachine;
    
    public enum Status { Success, Fail }
    public enum AnimationStatus { Start, End }

    public void Connect(EnemyController controller, BTMachine btMachine)
    {
        this.controller = controller;
        this.btMachine = btMachine;
    }

    public void SetStatus(Status newStatus)
    {
        btMachine.SetNode(null); // 상태를 비우거나, 변경을 위한 정지 필요
        parent.GetStatus(newStatus);
    }

    public virtual void Start() {}
    public virtual void Update() {}
    public virtual void End() {}

    public virtual void OnAnimatedEvent(bool isFire) {}
    public virtual void OnAnimated(AnimationStatus status, Animator animator) {}
    
    public virtual void GetStatus(Status newStatus) {}
}
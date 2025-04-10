using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    protected Node parent;
    protected List<Node> children = new();
    
    protected EnemyController controller; 
    protected BTMachine btMachine;
    public Cycle currCycle = Cycle.Finished; 
    
    public enum Status { Success, Fail }
    public enum Cycle { Started, Running, Finished }
    public enum AnimationStatus { Start, End }

    public void Connect(EnemyController controller)
    {
        this.controller = controller;
        this.btMachine = this.controller.btMachine;
    }

    public void SetParent(Node parent)
    {
        this.parent = parent;
    }

    public void SetStatus(Status newStatus)
    {
        btMachine.isRunning = false; // 일시 정지, 캡슐화 필요
        parent.GetStatus(newStatus, this);
    }

    public virtual void Start() {}
    public virtual void Update() {}
    public virtual void End() {}

    public virtual void OnAnimatedEvent(bool isFire) {}
    public virtual void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo) {}
    
    public virtual void GetStatus(Status newStatus, Node caller) {}
}
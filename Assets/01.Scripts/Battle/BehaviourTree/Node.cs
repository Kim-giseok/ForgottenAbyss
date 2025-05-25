using System.Collections.Generic;
using UnityEngine;

// 비제네릭 베이스 클래스
public abstract class Node
{
    private Node parent;
    public List<Node> children { get; private set; } = new();

    protected EnemyBaseController controller;
    protected BTMachine machine;
    protected BTContext context;

    protected float currTime => machine.currTime;

    protected enum Status { Success, Fail }
    public enum AnimationStatus { Start, End }

    protected bool IsIgnoreNotify = false;

    public Node Ignore()
    {
        IsIgnoreNotify = true;
        return this;
    }

    public void SetController(EnemyBaseController controller)
    {
        this.controller = controller;
        machine = controller.Machine;
        context = machine.context;
    }

    public void SetParent(Node parent)
    {
        this.parent = parent;
    }

    public void AddChild(Node child)
    {
        child.SetParent(this);
        children.Add(child);
    }

    protected virtual void GetStatus(Status newStatus, Node caller) { }

    protected void SetStatus(Status newStatus)
    {
        machine.SetPlaying(false);

        parent.SetController(controller);
        parent.GetStatus(newStatus, this);
    }

    public virtual void Start() { }
    public virtual void Update() { }
    public virtual void End() { }

    public virtual void OnAnimatedEvent(bool isFire) { }
    public virtual void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo) { }

    public virtual void OnPressed() { }
}

// 제네릭 서브 클래스
public abstract class Node<T> : Node where T : EnemyBaseController
{
    // 새로 제네릭 타입으로 컨트롤러 선언 (부모 필드 숨김)
    protected new T controller => base.controller as T;
}

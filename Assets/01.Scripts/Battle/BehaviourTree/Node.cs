using UnityEngine;

public abstract class Node
{
    protected Node parent;
    protected EnemyController controller; 
    protected BTMachine btMachine;

    public void Connect(EnemyController controller, BTMachine btMachine)
    {
        this.controller = controller;
        this.btMachine = btMachine;
    }
    

    public virtual void Start() {}
    public virtual void Update() {}
    public virtual void End() {}

    public virtual void OnAnimatedEvent(bool isFire) {}
    public virtual void OnAnimated(Animator animator) {}
}
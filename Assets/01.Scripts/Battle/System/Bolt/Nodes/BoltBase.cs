using UnityEngine;

public class BoltNode
{
    public StepMachine machine;
    public Bolt controller;

    public void Connect(StepMachine machine, Bolt controller)
    {
        this.machine = machine;
        this.controller = controller;
    }
    
    public virtual void Start() {}
    public virtual void Update() {}
    public virtual void End() {}

    // 다음 노드로 변경
    public void Next() {}
}


public abstract class BoltEffect
{
    public Bolt controller { get; private set; }
    public void Connect(Bolt controller) => this.controller = controller;
    public abstract void Execute(Collider2D other);
}

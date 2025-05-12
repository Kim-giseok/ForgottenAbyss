using UnityEngine;

public class BoltNode
{
    private StepMachine machine;
    protected BoltBuilder bolt;

    protected float currTime => bolt.currTime - machine.snapshotTime;
    public BoltContext context = new();

    protected void Next() => machine.Next();

    public void Connect(StepMachine machine)
    {
        this.machine = machine;
        bolt = this.machine.bolt;
    }

    public void Awake() { machine.snapshotTime = bolt.currTime; }
    
    public virtual void Start() {}
    public virtual void Update() {}
    public virtual void End() {}
}


public abstract class BoltEffect
{
    public BoltBuilder bolt { get; private set; }
    public void Connect(BoltBuilder controller) => this.bolt = controller;
    public abstract void Execute(Collider2D other);
}

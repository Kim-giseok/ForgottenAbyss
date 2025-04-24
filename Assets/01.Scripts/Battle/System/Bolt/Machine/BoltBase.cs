using UnityEngine;

public class BoltNode
{
    public StepMachine machine;
    public Bolt bolt;
    
    public float currTime => bolt.currTime - machine.snapshotTime;
    public BoltContext context = new();

    public void Next() => machine.Next();

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
    public Bolt bolt { get; private set; }
    public void Connect(Bolt controller) => this.bolt = controller;
    public abstract void Execute(Collider2D other);
}

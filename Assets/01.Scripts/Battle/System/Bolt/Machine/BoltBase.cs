using UnityEngine;

public class BoltNode
{
    public StepMachine machine;
    public Bolt bolt;
    
    // 노드별 개별 타임 체크 필요 
    public float currTime;
    public float time
    {
        get
        {
            return bolt.currTime;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void Next() => machine.Next();

    public void Connect(StepMachine machine)
    {
        this.machine = machine;
        bolt = this.machine.bolt;
    }
    
    public virtual void Start() {}
    public virtual void Update() {}
    public virtual void End() {}
}


public abstract class BoltEffect
{
    public Bolt controller { get; private set; }
    public void Connect(Bolt controller) => this.controller = controller;
    public abstract void Execute(Collider2D other);
}

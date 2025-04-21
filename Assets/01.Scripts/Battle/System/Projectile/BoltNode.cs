// OnTriggerEnter 부분
public class BoltNode
{
    public BoltStepMachine machine;
    public Bolt bolt;
    
    public virtual void Start() {}
    public virtual void Update() {}
    public virtual void End() {}

    // 다음 노드로 변경
    public void Next() {}
}
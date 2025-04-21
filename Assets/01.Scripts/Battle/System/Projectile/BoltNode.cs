// OnTriggerEnter 부분
public class BoltNode
{
    public BoltStepMachine machine;
    public BoltController BoltController;
    
    public virtual void Start() {}
    public virtual void Update() {}
    public virtual void End() {}

    // 다음 노드로 변경
    public void Next() {}
}

public class BoltGuidedNode : BoltNode {}
public class BoldParabolaNode: BoltNode {}

public class BoltWaitNode : BoltNode {}

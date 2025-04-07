using UnityEngine;

public class IdleNode : Node
{
    private float currTime;

    public override void Start()
    {
        currTime = 0;
    }

    public override void Update()
    {
        currTime += Time.deltaTime;
    }
}

public class PatrolNode: Node {}

// 무기가 Node를 결정할 수 있도록
public class KnifeAttackNode: Node {} 

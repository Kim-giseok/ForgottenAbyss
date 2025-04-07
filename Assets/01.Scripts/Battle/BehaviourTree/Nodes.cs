using UnityEngine;

public class IdleNode : Node
{
    private float currTime;
    public float duration;

    public IdleNode(float duration)
    {
        this.duration = duration;
    }

    public override void Start()
    {
        currTime = 0;
    }

    public override void Update()
    {
        currTime += Time.deltaTime;
        if (currTime >= duration)
        {
            SetStatus(Status.Success);
            return;
        }
    }
}

public class PatrolNode : Node
{
    private float currTime;
    public float duration;
    private Vector2 direction;

    public PatrolNode(float duration)
    {
        this.duration = duration;
    }

    public override void Start()
    {
        currTime = 0;
        direction = Random.Range(0, 10) <= 5 ? Vector2.left : Vector2.right;
    }

    public override void Update()
    {
        currTime += Time.deltaTime;
        if (currTime >= duration)
        {
            SetStatus(Status.Success);
            return;
        }
    }
}

// 무기가 Node를 결정할 수 있도록
public class KnifeAttackNode: Node {} 

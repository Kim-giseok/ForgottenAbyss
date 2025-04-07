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
        Debug.Log("idle");
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
        controller.Flip(direction == Vector2.right);
    }

    public override void Update()
    {
        currTime += Time.deltaTime;
        if (currTime >= duration)
        {
            SetStatus(Status.Success);
            return;
        }
        
        controller.rigidbody.velocity = new Vector2(direction.x, controller.rigidbody.velocity.y);
    }
}

public class TracingNode : Node
{
    public override void Start()
    {
        if (controller.agent.detectedDistance < controller.agent.GetDistance())  
        {
            SetStatus(Status.Fail);
            return;
        }
    }

    public override void Update()
    {
        Debug.Log(controller.agent.GetDistance());
        if (controller.agent.detectedDistance < controller.agent.GetDistance())
        {
            SetStatus(Status.Fail);
            return;
        }
        
        if (controller.agent.stopingDistance > controller.agent.GetDistance())
        {
            SetStatus(Status.Success);
           return;
        }
    }
}

// 무기가 Node를 결정할 수 있도록
public class KnifeAttackNode : Node
{
    public override void Update()
    {
        if (controller.agent.stopingDistance < controller.agent.GetDistance())
        {
            SetStatus(Status.Fail);
            return;
        }
        
        Debug.Log("attack");
    }
} 



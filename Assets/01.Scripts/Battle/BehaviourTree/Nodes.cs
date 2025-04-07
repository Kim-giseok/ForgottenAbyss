using Unity.VisualScripting;
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
        Vector2 distance = (controller.agent.player.transform.position - controller.transform.position);
        
        if (controller.agent.detectedDistance >= distance.magnitude)
        {
            SetStatus(Status.Fail);
            return;
        }

        
        currTime += Time.deltaTime;
        if (currTime >= duration)
        {
            SetStatus(Status.Success);
            return;
        }
        
        Debug.Log("idle");
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
        Vector2 distance = (controller.agent.player.transform.position - controller.transform.position);
        
        if (controller.agent.detectedDistance >= distance.magnitude)
        {
            SetStatus(Status.Fail);
            return;
        }
        
        currTime += Time.deltaTime;
        if (currTime >= duration)
        {
            SetStatus(Status.Success);
            return;
        }
        
        controller.rigidbody.velocity = new Vector2(direction.x, controller.rigidbody.velocity.y);
        Debug.Log("Patrol");
    }
}

public class TracingNode : Node
{
    public override void Update()
    {
        Vector2 distance = (controller.agent.player.transform.position - controller.transform.position);
        controller.Flip(distance.normalized.x > 0);
        
        if (controller.agent.detectedDistance < distance.magnitude)
        {
            Debug.Log("fail");
            SetStatus(Status.Fail);
            return;
        }
        
        if (controller.agent.stoppingDistance > distance.magnitude)
        {
            Debug.Log("success");
            SetStatus(Status.Success);
           return;
        }
        
        controller.rigidbody.velocity = new Vector2(distance.normalized.x * controller.agent.tracingSpeed, controller.rigidbody.velocity.y);
        Debug.Log("tracing");
    }
}

// 무기가 Node를 결정할 수 있도록
public class KnifeAttackNode : Node
{
    public override void Update()
    {
        float distance = (controller.transform.position - controller.agent.player.transform.position).magnitude;

        if (controller.agent.stoppingDistance < distance)
        {
            SetStatus(Status.Fail);
            return;
        }
        
        Debug.Log("attack");
    }
}

public class HitNode : Node
{
    public override void Start()
    {
        if (!controller.isHit)
        {
            SetStatus(Status.Fail);
            return;
        }
        
        controller.isHit = false;
    }
}
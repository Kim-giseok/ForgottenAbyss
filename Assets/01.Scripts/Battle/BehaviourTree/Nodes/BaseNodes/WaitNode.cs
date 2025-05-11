using BT;
using UnityEngine;

public class WaitNode : Node
{
    private readonly Wait command;
    
    public WaitNode(Wait wait) => this.command = wait;

    public override void Start()
    {
        if (command.isStopPos)
        {
            controller.Rigidbody.velocity = new Vector2(0, controller.Rigidbody.velocity.y);
        }
        controller.animnHandler.Play(command.animName);
    }

    public override void Update()
    {
        // 대상을 바라보게 해야하는 경우
        if (command.isLookTarget) { controller.LookTarget(); } 
    }
}
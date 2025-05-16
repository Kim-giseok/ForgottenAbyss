using UnityEngine;

public class Test1Node : Node
{
    private bool isFlipEnd;
    public Test1Node(bool isFlipEnd)
    {
        this.isFlipEnd = isFlipEnd;
    }
    public override void Start()
    {
        controller.animHandler.Play("Run");
        controller.Flip(false);
    }

    public override void Update()
    {
        if (currTime > 2)
        {
            SetStatus(Status.Success);
            return;
        }
        
        var currVelocity = controller.Rigidbody.velocity;
        controller.Rigidbody.velocity = new Vector2(-3, currVelocity.y);
    }

    public override void End()
    {
        controller.Rigidbody.velocity = Vector2.zero;
        controller.Flip(isFlipEnd);
    }
}
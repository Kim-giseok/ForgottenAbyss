using UnityEngine;

public class MudMoveNode : Node
{
    private readonly Vector2 direction;
    
    public MudMoveNode(Vector2 direction) => this.direction = direction;
    public override void Start()
    {
        controller.rigidbody.AddForce(direction, ForceMode2D.Impulse);
    }

    public override void Update()
    {
        if(currTime > 0.2f) { SetStatus(Status.Success); }
    }

    public override void End()
    {
        controller.rigidbody.velocity = Vector2.zero;
    }
}

public class MudWarpNode : Node
{
    public override void Start()
    {
        controller.animnHandler.Play("Warp");
        float clampedX = Random.Range(-7, 7);
        float clampedY = Random.Range(-5, 5);
        controller.transform.position = new Vector2(clampedX, clampedY);
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Warp")) return;
        if (status == AnimationStatus.End) { SetStatus(Status.Success); }
    }
}

public class MudSpawnNode : Node
{
    public override void Start()
    {
        var currPos = NavSurface.Instance.GetPlatform(controller.agent.target).centerCell.WorldPos + new Vector2(0, 1.7f);
        EnemyRespawnManager.Instance.Generate(Enemies.Enemy.MudChildHand, currPos);
        SetStatus(Status.Success);
    }
}

public class MudControlnode : Node
{
    public override void Update()
    {
        if (controller is not EnemyController eController) return;
        // Debug.Log(eController.children.Count);
        
        // eController.children[0].transform.localScale = Vector3.Lerp(eController.children[0].transform.localScale, Vector3.one * 2f, 0.1f);
    }
}
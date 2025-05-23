using UnityEngine;

public class MudMoveNode : Node
{
    private readonly Vector2 direction;
    
    public MudMoveNode(Vector2 direction) => this.direction = direction;
    public override void Start()
    {
        controller.Rigid.AddForce(direction, ForceMode2D.Impulse);
    }

    public override void Update()
    {
        if(currTime > 0.2f) { SetStatus(Status.Success); }
    }

    public override void End()
    {
        controller.Rigid.velocity = Vector2.zero;
    }
}

public class MudWarpNode : Node
{
    public override void Start()
    {
        controller.Anim.Play("Warp");
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

public class MudEyeSpell : Node
{
    public override void Start()
    {
        controller.Anim.Play("Attack");
        
        controller.transform.localScale = new Vector3(Random.Range(0.8f, 3f), Random.Range(0.8f, 3f), 1);
        
        SoundManager.Instance.Playsfx("Piano1");
        var count = 8;
        for (var i = 0; i < count; i++)
        {
            var angle = i * 360f / count;
            var dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;

            BoltsPool.Instance
                .Create(controller.transform, Bolts.Type.Linear)
                .SetDirection(dir)
                .SetSpeed(16f)
                .SetDamage(30)
                .Fire();
        }
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (animInfo.IsName("Attack") && status == AnimationStatus.End) { SetStatus(Status.Success); }
     }
}